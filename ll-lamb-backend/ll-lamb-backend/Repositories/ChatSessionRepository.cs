using Microsoft.EntityFrameworkCore;
using LlLambBackend.Data;
using LlLambBackend.Models;

namespace LlLambBackend.Repositories;

public class ChatSessionRepository : IChatSessionRepository
{
    private readonly ApplicationDbContext _context;
    
    public ChatSessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<ChatSession?> GetByIdAsync(Guid sessionId)
    {
        return await _context.ChatSessions
            .Include(cs => cs.Model)
            .Include(cs => cs.User)
            .FirstOrDefaultAsync(cs => cs.SessionId == sessionId && cs.IsActive);
    }
    
    public async Task<List<ChatSession>> GetByUserIdAsync(Guid userId)
    {
        return await _context.ChatSessions
            .Include(cs => cs.Model)
            .Where(cs => cs.UserId == userId && cs.IsActive)
            .OrderByDescending(cs => cs.UpdatedAt)
            .ToListAsync();
    }
    
    public async Task<ChatSession> CreateAsync(ChatSession chatSession)
    {
        chatSession.SessionId = Guid.NewGuid();
        chatSession.CreatedAt = DateTime.UtcNow;
        chatSession.UpdatedAt = DateTime.UtcNow;
        
        _context.ChatSessions.Add(chatSession);
        await _context.SaveChangesAsync();
        return chatSession;
    }
    
    public async Task<ChatSession> UpdateAsync(ChatSession chatSession)
    {
        chatSession.UpdatedAt = DateTime.UtcNow;
        _context.ChatSessions.Update(chatSession);
        await _context.SaveChangesAsync();
        return chatSession;
    }
    
    public async Task<bool> DeleteAsync(Guid sessionId)
    {
        var session = await GetByIdAsync(sessionId);
        if (session == null) return false;
        
        session.IsActive = false;
        session.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}