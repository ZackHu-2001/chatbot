using System.ComponentModel.DataAnnotations;

namespace LlLambBackend.DTOs;

public class SendMessageDto
{
    [Required]
    public Guid SessionId { get; set; }
    
    [Required]
    [StringLength(10000, MinimumLength = 1)]
    public string UserMessage { get; set; } = string.Empty;
}