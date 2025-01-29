using CSharpFunctionalExtensions;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.RefinancePayrollLoan;

public sealed record RefinancePayrollLoanCommand : IRequest<Result<PayrollLoan>>
{
    public Guid Id { get; }
    public decimal Amount { get; }
    public int NumberOfInstallments { get; }

    private RefinancePayrollLoanCommand(Guid id, decimal amount, int numberOfInstallments)
    {
        Id = id;
        Amount = amount;
        NumberOfInstallments = numberOfInstallments;
    }

    public static Result<RefinancePayrollLoanCommand> Create(Guid id, decimal amount, int numberOfInstallments)
    {
        if (amount < 100)
            return Result.Failure<RefinancePayrollLoanCommand>("Minimum amount of payroll loan is 100");

        if (numberOfInstallments < 1)
            return Result.Failure<RefinancePayrollLoanCommand>("Number of installments must be greater than 0");

        return new RefinancePayrollLoanCommand(id, amount, numberOfInstallments);
    }
}

public sealed class RefinancePayrollLoanCommandHandler : IRequestHandler<RefinancePayrollLoanCommand, Result<PayrollLoan>>
{
    private readonly PayrollLoansRepository _repository;

    public RefinancePayrollLoanCommandHandler(PayrollLoansRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PayrollLoan>> Handle(RefinancePayrollLoanCommand command, CancellationToken cancellationToken)
    {
        var payrollLoan = await _repository.GetPayrollLoanByIdAsync(command.Id);
        if (payrollLoan.HasNoValue)
            return Result.Failure<PayrollLoan>("Payroll loan not found");

        payrollLoan.Value.Refinance(command.Amount, command.NumberOfInstallments);
        
        await _repository.Save(payrollLoan.Value, cancellationToken);
        return payrollLoan.Value;
    }
}
