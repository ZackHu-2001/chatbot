using LlLambBackend.Models;

namespace LlLambBackend.DTOs;

public class MessageDto
{
    public Guid MessageId { get; set; }
    public MessageRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int? TokenCount { get; set; }
}