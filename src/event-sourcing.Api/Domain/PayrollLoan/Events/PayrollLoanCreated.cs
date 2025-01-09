using System.Text.Json.Serialization;

namespace event_sourcing.Domain.PayrollLoan.Events;

public record PayrollLoanCreated(Guid PayrollLoanId, decimal Amount, decimal InterestRate, int NumberOfInstallments) : Event
{
    [JsonIgnore]
    public override Guid StreamId => PayrollLoanId;
}
