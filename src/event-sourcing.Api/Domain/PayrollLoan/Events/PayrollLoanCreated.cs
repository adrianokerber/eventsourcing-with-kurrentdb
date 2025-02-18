using System.Text.Json.Serialization;
using event_sourcing.Domain.Shared;

namespace event_sourcing.Domain.PayrollLoan.Events;

public record PayrollLoanCreated(Guid PayrollLoanId, decimal Amount, decimal InterestRate, int NumberOfInstallments, DateTime CreatedAtUtc) : Event
{
    [JsonIgnore]
    public override Guid StreamId => PayrollLoanId;
}
