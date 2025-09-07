namespace LlLambBackend.DTOs;

public class ChatSessionDto
{
    public Guid SessionId { get; set; }
    public string? Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ModelDto Model { get; set; } = null!;
}