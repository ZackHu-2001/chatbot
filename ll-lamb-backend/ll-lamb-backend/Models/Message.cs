using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LlLambBackend.Models;

public enum MessageRole
{
    User,
    Assistant,
    System
}

public class Message
{
    [Key]
    public Guid MessageId { get; set; }
    
    [Required]
    public Guid SessionId { get; set; }
    
    [Required]
    public MessageRole Role { get; set; }
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int? TokenCount { get; set; }
    
    [ForeignKey(nameof(SessionId))]
    public ChatSession ChatSession { get; set; } = null!;
}