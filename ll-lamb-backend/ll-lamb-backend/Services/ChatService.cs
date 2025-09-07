using LlLambBackend.DTOs;
using LlLambBackend.Models;
using LlLambBackend.Repositories;

namespace LlLambBackend.Services;

public class ChatService : IChatService
{
    private readonly IModelRepository _modelRepository;
    private readonly IChatSessionRepository _chatSessionRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IAzureOpenAIService _openAIService;
    private readonly ILogger<ChatService> _logger;
    
    public ChatService(
        IModelRepository modelRepository,
        IChatSessionRepository chatSessionRepository,
        IMessageRepository messageRepository,
        IAzureOpenAIService openAIService,
        ILogger<ChatService> logger)
    {
        _modelRepository = modelRepository;
        _chatSessionRepository = chatSessionRepository;
        _messageRepository = messageRepository;
        _openAIService = openAIService;
        _logger = logger;
    }
    
    public async Task<List<ModelDto>> GetModelsAsync()
    {
        var models = await _modelRepository.GetAllActiveAsync();
        return models.Select(m => new ModelDto
        {
            ModelId = m.ModelId,
            Name = m.Name,
            Provider = m.Provider,
            Status = m.Status
        }).ToList();
    }
    
    public async Task<ChatSessionDto?> CreateSessionAsync(Guid userId, CreateSessionDto createSessionDto)
    {
        var model = await _modelRepository.GetByIdAsync(createSessionDto.ModelId);
        if (model == null || !model.Status)
        {
            return null;
        }
        
        var chatSession = new ChatSession
        {
            UserId = userId,
            ModelId = createSessionDto.ModelId,
            Title = createSessionDto.Title
        };
        
        var createdSession = await _chatSessionRepository.CreateAsync(chatSession);
        
        return new ChatSessionDto
        {
            SessionId = createdSession.SessionId,
            Title = createdSession.Title,
            CreatedAt = createdSession.CreatedAt,
            UpdatedAt = createdSession.UpdatedAt,
            Model = new ModelDto
            {
                ModelId = model.ModelId,
                Name = model.Name,
                Provider = model.Provider,
                Status = model.Status
            }
        };
    }
    
    public async Task<List<ChatSessionDto>> GetUserSessionsAsync(Guid userId)
    {
        var sessions = await _chatSessionRepository.GetByUserIdAsync(userId);
        return sessions.Select(s => new ChatSessionDto
        {
            SessionId = s.SessionId,
            Title = s.Title,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt,
            Model = new ModelDto
            {
                ModelId = s.Model.ModelId,
                Name = s.Model.Name,
                Provider = s.Model.Provider,
                Status = s.Model.Status
            }
        }).ToList();
    }
    
    public async Task<ChatResponseDto?> SendMessageAsync(Guid userId, SendMessageDto sendMessageDto)
    {
        var session = await _chatSessionRepository.GetByIdAsync(sendMessageDto.SessionId);
        if (session == null || session.UserId != userId)
        {
            return null;
        }
        
        var userMessage = new Message
        {
            SessionId = sendMessageDto.SessionId,
            Role = MessageRole.User,
            Content = sendMessageDto.UserMessage
        };
        
        await _messageRepository.CreateAsync(userMessage);
        
        try
        {
            var chatHistory = await _messageRepository.GetBySessionIdAsync(sendMessageDto.SessionId);
            var messages = chatHistory.Select(m => (m.Role.ToString(), m.Content)).ToList();
            
            var assistantResponse = await _openAIService.GetChatCompletionAsync(
                session.Model.Endpoint,
                session.Model.ApiKey,
                messages);
                
            var assistantMessage = new Message
            {
                SessionId = sendMessageDto.SessionId,
                Role = MessageRole.Assistant,
                Content = assistantResponse
            };
            
            var savedMessage = await _messageRepository.CreateAsync(assistantMessage);
            
            session.UpdatedAt = DateTime.UtcNow;
            await _chatSessionRepository.UpdateAsync(session);
            
            return new ChatResponseDto
            {
                AssistantMessage = assistantResponse,
                MessageId = savedMessage.MessageId,
                CreatedAt = savedMessage.CreatedAt,
                TokenCount = savedMessage.TokenCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating AI response for session {SessionId}", sendMessageDto.SessionId);
            
            var errorMessage = new Message
            {
                SessionId = sendMessageDto.SessionId,
                Role = MessageRole.Assistant,
                Content = "抱歉，我现在无法处理您的请求。请稍后再试。"
            };
            
            var savedErrorMessage = await _messageRepository.CreateAsync(errorMessage);
            
            return new ChatResponseDto
            {
                AssistantMessage = errorMessage.Content,
                MessageId = savedErrorMessage.MessageId,
                CreatedAt = savedErrorMessage.CreatedAt
            };
        }
    }
    
    public async Task<List<MessageDto>> GetChatHistoryAsync(Guid sessionId, Guid userId)
    {
        var session = await _chatSessionRepository.GetByIdAsync(sessionId);
        if (session == null || session.UserId != userId)
        {
            return new List<MessageDto>();
        }
        
        var messages = await _messageRepository.GetBySessionIdAsync(sessionId);
        return messages.Select(m => new MessageDto
        {
            MessageId = m.MessageId,
            Role = m.Role,
            Content = m.Content,
            CreatedAt = m.CreatedAt,
            TokenCount = m.TokenCount
        }).ToList();
    }
}