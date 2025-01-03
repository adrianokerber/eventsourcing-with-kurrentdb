using System.Text.Json.Serialization;

namespace event_sourcing.Domain.PayrollLoan.Events;

[JsonPolymorphic]
[JsonDerivedType(typeof(PayrollLoanCreated), nameof(PayrollLoanCreated))]
public abstract record Event
{
    public abstract Guid StreamId { get; }
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}