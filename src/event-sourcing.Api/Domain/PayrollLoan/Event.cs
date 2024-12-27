namespace event_sourcing.Domain.PayrollLoan;

public sealed record Event
{
    public required string Id { get; init; }
    public required string Type { get; init; }
    public required string Data { get; init; }
    public DateTime CreatedAt { get; init; }
}