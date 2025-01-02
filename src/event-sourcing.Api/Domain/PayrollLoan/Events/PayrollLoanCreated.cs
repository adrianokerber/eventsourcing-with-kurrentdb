using System.Text.Json;

namespace event_sourcing.Domain.PayrollLoan.Events;

public record PayrollLoanCreated(Guid PayrollLoanId, decimal Amount, decimal InterestRate, int TermMonths) : Event
{
    public override Guid StreamId => PayrollLoanId;
    public override string Type => nameof(PayrollLoanCreated);
    public override string Data => JsonSerializer.Serialize(new { PayrollLoanId, Amount, InterestRate, TermMonths });
}
