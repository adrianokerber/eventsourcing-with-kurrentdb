using System.Text.Json.Serialization;
using event_sourcing.Domain.Shared;

namespace event_sourcing.Domain.PayrollLoan.Events;

public record PayrollLoanRefinanced(Guid PayrollLoanId, decimal Amount, int NumberOfInstallments) : Event
{
    [JsonIgnore]
    public override Guid StreamId => PayrollLoanId;
}