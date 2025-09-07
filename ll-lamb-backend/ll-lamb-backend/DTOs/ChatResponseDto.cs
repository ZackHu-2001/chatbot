namespace LlLambBackend.DTOs;

public class ChatResponseDto
{
    public string AssistantMessage { get; set; } = string.Empty;
    public Guid MessageId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? TokenCount { get; set; }
}