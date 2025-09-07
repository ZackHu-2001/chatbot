using LlLambBackend.DTOs;

namespace LlLambBackend.Services;

public interface IChatService
{
    Task<List<ModelDto>> GetModelsAsync();
    Task<ChatSessionDto?> CreateSessionAsync(Guid userId, CreateSessionDto createSessionDto);
    Task<List<ChatSessionDto>> GetUserSessionsAsync(Guid userId);
    Task<ChatResponseDto?> SendMessageAsync(Guid userId, SendMessageDto sendMessageDto);
    Task<List<MessageDto>> GetChatHistoryAsync(Guid sessionId, Guid userId);
}