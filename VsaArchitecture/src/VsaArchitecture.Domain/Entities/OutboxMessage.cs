namespace VsaArchitecture.Domain.Entities;

public class OutboxMessage
{
    public int Id { get; set; }
    public required string EventType { get; set; }
    public required string Message { get; set; }
    public DateTime PostedOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}