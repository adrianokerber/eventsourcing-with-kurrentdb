namespace event_sourcing.Domain.PayrollLoan;

public sealed class Event
{
    public required string Id { get; set; }
    public required string Type { get; set; }
    public required string Data { get; set; }
    public DateTime CreatedAt { get; set; }
}