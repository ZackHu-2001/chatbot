using LlLambBackend.Models;

namespace LlLambBackend.Repositories;

public interface IChatSessionRepository
{
    Task<ChatSession?> GetByIdAsync(Guid sessionId);
    Task<List<ChatSession>> GetByUserIdAsync(Guid userId);
    Task<ChatSession> CreateAsync(ChatSession chatSession);
    Task<ChatSession> UpdateAsync(ChatSession chatSession);
    Task<bool> DeleteAsync(Guid sessionId);
}