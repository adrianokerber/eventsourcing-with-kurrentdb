using System.Text.Json.Serialization;
using event_sourcing.Domain.PayrollLoan.Events;

namespace event_sourcing.Domain.Shared;

// TODO: change by an interface to avoid Json serialization specific configuration. Also remove CreateAtUtc from Domain since we don't need it.
[JsonPolymorphic]
[JsonDerivedType(typeof(PayrollLoanCreated), nameof(PayrollLoanCreated))]
[JsonDerivedType(typeof(PayrollLoanRefinanced), nameof(PayrollLoanRefinanced))]
public abstract record Event
{
    public abstract Guid StreamId { get; }
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}