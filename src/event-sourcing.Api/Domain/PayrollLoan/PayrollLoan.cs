using event_sourcing.Domain.PayrollLoan.Events;
using event_sourcing.Domain.Shared;

namespace event_sourcing.Domain.PayrollLoan;

using CSharpFunctionalExtensions;

public sealed class PayrollLoan
{
    public IReadOnlyList<Event> NewEvents => _newEvents;
    private readonly List<Event> _newEvents = new List<Event>();

    private PayrollLoan(Event payrollLoanEvent)
    {
        _newEvents.Add(payrollLoanEvent);
        Apply(payrollLoanEvent);
    }
    
    public PayrollLoan(List<Event> domainEvents)
    {
        foreach (var @event in domainEvents)
            Apply(@event);
    }

    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public decimal InterestRate { get; private set; }
    public int NumberOfInstallments { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public static Result<PayrollLoan> Create(
        decimal amount,
        decimal interestRate,
        int numberOfInstallments)
    {
        if (amount <= 0)
            return Result.Failure<PayrollLoan>("Amount must be greater than zero");

        if (interestRate <= 0)
            return Result.Failure<PayrollLoan>("Interest rate must be greater than zero");

        if (numberOfInstallments <= 0)
            return Result.Failure<PayrollLoan>("Number of installments must be greater than zero");

        if (numberOfInstallments > 72)
            return Result.Failure<PayrollLoan>("Number of installments cannot exceed 72 months");
        
        // TODO: insert event on aggregate
        var id = Guid.NewGuid();
        var createdAtUtc = DateTime.UtcNow;
        var @event = new PayrollLoanCreated(id, amount, interestRate, numberOfInstallments, createdAtUtc);

        var payrollLoan = new PayrollLoan(@event);
        
        return Result.Success(payrollLoan);
    }

    public void Refinance(decimal amount, int numberOfInstallments)
    {
        var @event = new PayrollLoanRefinanced(Id, amount, numberOfInstallments);
        
        _newEvents.Add(@event);
        Apply(@event);
    }
    
    private void Apply(Event @event)
    {
        // TODO: Remove necessity of writing every type
        switch (@event)
        {
            case PayrollLoanCreated ev:
                Apply(ev);
                break;
            case PayrollLoanRefinanced ev:
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

    private void Apply(PayrollLoanRefinanced @event)
    {
        Amount = @event.Amount;
        NumberOfInstallments = @event.NumberOfInstallments;
    }
}