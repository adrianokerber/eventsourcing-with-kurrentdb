namespace event_sourcing.Domain.PayrollLoan;

using CSharpFunctionalExtensions;

public sealed class PayrollLoan
{
    private PayrollLoan(
        Guid id,
        decimal amount,
        decimal interestRate,
        int numberOfInstallments,
        DateTime createdAtUtc,
        DateTime? updatedAtUtc = null)
    {
        Id = id;
        Amount = amount;
        InterestRate = interestRate;
        NumberOfInstallments = numberOfInstallments;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = updatedAtUtc;
    }

    public Guid Id { get; private init; }
    public decimal Amount { get; private init; }
    public decimal InterestRate { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreatedAtUtc { get; private init; }
    public DateTime? UpdatedAtUtc { get; private init; }

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

        var payrollLoan = new PayrollLoan(
            id: Guid.NewGuid(),
            amount: amount,
            interestRate: interestRate,
            numberOfInstallments: numberOfInstallments,
            createdAtUtc: DateTime.UtcNow);
        
        // TODO: add domain event here!

        return Result.Success(payrollLoan);
    }

    // TODO: apply the concept of apply method to build the aggregate using the events
    // private Apply(PayrollLoanCreated @event)
    // {
    //     Id = @event.Id;
    // }
}