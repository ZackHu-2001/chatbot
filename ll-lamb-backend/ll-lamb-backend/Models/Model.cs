using System.ComponentModel.DataAnnotations;

namespace LlLambBackend.Models;

public class Model
{
    [Key]
    public Guid ModelId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Provider { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Endpoint { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string ApiKey { get; set; } = string.Empty;
    
    public bool Status { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();
}