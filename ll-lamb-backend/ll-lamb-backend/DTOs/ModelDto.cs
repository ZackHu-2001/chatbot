namespace LlLambBackend.DTOs;

public class ModelDto
{
    public Guid ModelId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public bool Status { get; set; }
}