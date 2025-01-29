using CSharpFunctionalExtensions;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

public sealed record CreatePayrollLoanCommand : IRequest<Result<PayrollLoan>>
{
    public decimal Amount { get; }
    public decimal InterestRate { get; }
    public int NumberOfInstallments { get; }

    private CreatePayrollLoanCommand(decimal amount, decimal interestRate, int numberOfInstallments)
    {
        Amount = amount;
        InterestRate = interestRate;
        NumberOfInstallments = numberOfInstallments;
    }

    public static Result<CreatePayrollLoanCommand> Create(decimal amount, decimal interestRate, int numberOfInstallments)
    {
        if (amount < 100)
            return Result.Failure<CreatePayrollLoanCommand>("Minimum amount of payroll loan is 100");

        if (interestRate <= 1)
            return Result.Failure<CreatePayrollLoanCommand>("Interest rate must be greater than 1");

        if (numberOfInstallments < 1)
            return Result.Failure<CreatePayrollLoanCommand>("Number of installments must be greater than 0");

        return new CreatePayrollLoanCommand(amount, interestRate, numberOfInstallments);
    }
}

public sealed class CreatePayrollLoanCommandHandler : IRequestHandler<CreatePayrollLoanCommand, Result<PayrollLoan>>
{
    private readonly PayrollLoansRepository _repository;

    public CreatePayrollLoanCommandHandler(PayrollLoansRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PayrollLoan>> Handle(CreatePayrollLoanCommand command, CancellationToken cancellationToken)
    {
        var (_, isFailure, payrollLoan, error) = PayrollLoan.Create(command.Amount, command.InterestRate, command.NumberOfInstallments);
        if (isFailure)
            return Result.Failure<PayrollLoan>(error);

        await _repository.Save(payrollLoan, cancellationToken);
        
        return payrollLoan;
    }
}
