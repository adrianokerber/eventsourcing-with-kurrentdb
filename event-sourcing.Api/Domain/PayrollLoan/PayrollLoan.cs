namespace event_sourcing.Domain.PayrollLoan;

using CSharpFunctionalExtensions;

public sealed record PayrollLoan
{
    private PayrollLoan(
        Guid id,
        decimal amount,
        decimal interestRate,
        int numberOfInstallments,
        DateTime createdAt,
        DateTime? updatedAt = null)
    {
        Id = id;
        Amount = amount;
        InterestRate = interestRate;
        NumberOfInstallments = numberOfInstallments;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; private init; }
    public decimal Amount { get; private init; }
    public decimal InterestRate { get; private init; }
    public int NumberOfInstallments { get; private init; }
    public DateTime CreatedAt { get; private init; }
    public DateTime? UpdatedAt { get; private init; }

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
            createdAt: DateTime.UtcNow);

        return Result.Success(payrollLoan);
    }
}