using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LlLambBackend.DTOs;
using LlLambBackend.Services;

namespace LlLambBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    
    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }
    
    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }
    
    [HttpPost("session")]
    public async Task<ActionResult<ChatSessionDto>> CreateSession(CreateSessionDto createSessionDto)
    {
        var userId = GetUserId();
        var session = await _chatService.CreateSessionAsync(userId, createSessionDto);
        
        if (session == null)
        {
            return BadRequest(new { message = "无法创建聊天会话，请检查模型是否可用" });
        }
        
        return Ok(session);
    }
    
    [HttpGet("sessions")]
    public async Task<ActionResult<List<ChatSessionDto>>> GetUserSessions()
    {
        var userId = GetUserId();
        var sessions = await _chatService.GetUserSessionsAsync(userId);
        return Ok(sessions);
    }
    
    [HttpPost("send")]
    public async Task<ActionResult<ChatResponseDto>> SendMessage(SendMessageDto sendMessageDto)
    {
        var userId = GetUserId();
        var response = await _chatService.SendMessageAsync(userId, sendMessageDto);
        
        if (response == null)
        {
            return BadRequest(new { message = "无法发送消息，请检查会话是否存在" });
        }
        
        return Ok(response);
    }
    
    [HttpGet("history/{sessionId}")]
    public async Task<ActionResult<List<MessageDto>>> GetChatHistory(Guid sessionId)
    {
        var userId = GetUserId();
        var messages = await _chatService.GetChatHistoryAsync(sessionId, userId);
        return Ok(messages);
    }
}