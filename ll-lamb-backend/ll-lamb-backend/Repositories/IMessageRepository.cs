using LlLambBackend.Models;

namespace LlLambBackend.Repositories;

public interface IMessageRepository
{
    Task<List<Message>> GetBySessionIdAsync(Guid sessionId);
    Task<Message> CreateAsync(Message message);
    Task<List<Message>> CreateBatchAsync(List<Message> messages);
    Task<Message?> GetByIdAsync(Guid messageId);
    Task<bool> DeleteAsync(Guid messageId);
}