using System.ComponentModel.DataAnnotations;

namespace LlLambBackend.DTOs;

public class CreateSessionDto
{
    [Required]
    public Guid ModelId { get; set; }
    
    public string? Title { get; set; }
}