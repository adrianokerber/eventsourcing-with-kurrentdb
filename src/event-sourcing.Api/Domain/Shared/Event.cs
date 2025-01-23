using System.Text.Json.Serialization;
using event_sourcing.Domain.PayrollLoan.Events;

namespace event_sourcing.Domain.Shared;

[JsonPolymorphic]
[JsonDerivedType(typeof(PayrollLoanCreated), nameof(PayrollLoanCreated))]
[JsonDerivedType(typeof(PayrollLoanRefinanced), nameof(PayrollLoanRefinanced))]
public abstract record Event // TODO: change for an interface and remove CreateAtUtc
{
    public abstract Guid StreamId { get; }
    public DateTime CreatedAtUtc { get; } = DateTime.UtcNow;
}