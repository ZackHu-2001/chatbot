using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LlLambBackend.Models;

public class ChatSession
{
    [Key]
    public Guid SessionId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Guid ModelId { get; set; }
    
    [MaxLength(100)]
    public string? Title { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public bool IsActive { get; set; } = true;
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    [ForeignKey(nameof(ModelId))]
    public Model Model { get; set; } = null!;
    
    public ICollection<Message> Messages { get; set; } = new List<Message>();
}