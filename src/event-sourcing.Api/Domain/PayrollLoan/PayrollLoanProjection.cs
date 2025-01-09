namespace event_sourcing.Domain.PayrollLoan;

using event_sourcing.Domain.PayrollLoan.Events;

public sealed class PayrollLoanProjection
{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public decimal InterestRate { get; private set; }
    public int NumberOfInstallments { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public void Apply(Event @event)
    {
        switch (@event)
        {
            case PayrollLoanCreated ev:
                Apply(ev);
                break;
        }
    }

    private void Apply(PayrollLoanCreated @event)
    {
        Id = @event.PayrollLoanId;
        Amount = @event.Amount;
        InterestRate = @event.InterestRate;
        NumberOfInstallments = @event.NumberOfInstallments;
        CreatedAtUtc = @event.CreatedAtUtc;
    }
}