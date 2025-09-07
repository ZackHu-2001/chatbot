using Microsoft.EntityFrameworkCore;
using LlLambBackend.Data;
using LlLambBackend.Models;

namespace LlLambBackend.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;
    
    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Message>> GetBySessionIdAsync(Guid sessionId)
    {
        return await _context.Messages
            .Where(m => m.SessionId == sessionId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<Message> CreateAsync(Message message)
    {
        message.MessageId = Guid.NewGuid();
        message.CreatedAt = DateTime.UtcNow;
        
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }
    
    public async Task<List<Message>> CreateBatchAsync(List<Message> messages)
    {
        foreach (var message in messages)
        {
            message.MessageId = Guid.NewGuid();
            message.CreatedAt = DateTime.UtcNow;
        }
        
        _context.Messages.AddRange(messages);
        await _context.SaveChangesAsync();
        return messages;
    }
    
    public async Task<Message?> GetByIdAsync(Guid messageId)
    {
        return await _context.Messages
            .FirstOrDefaultAsync(m => m.MessageId == messageId);
    }
    
    public async Task<bool> DeleteAsync(Guid messageId)
    {
        var message = await GetByIdAsync(messageId);
        if (message == null) return false;
        
        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
        return true;
    }
}