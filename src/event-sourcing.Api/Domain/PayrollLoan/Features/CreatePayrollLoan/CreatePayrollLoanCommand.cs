using CSharpFunctionalExtensions;
using event_sourcing.Domain.PayrollLoan.Events;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

public sealed record CreatePayrollLoanCommand : IRequest<Result<PayrollLoan>>
{
    public string EventName { get; }
    
    public decimal Amount { get; }
    public decimal InterestRate { get; }
    public int NumberOfInstallments { get; }

    private CreatePayrollLoanCommand(decimal amount, decimal interestRate, int numberOfInstallments)
    {
        EventName = "PayrollLoanCreated";
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

    public async Task<Result<PayrollLoan>> Handle(CreatePayrollLoanCommand request, CancellationToken cancellationToken)
    {
        var (_, isFailure, payrollLoan, error) = PayrollLoan.Create(request.Amount, request.InterestRate, request.NumberOfInstallments);
        if (isFailure)
            return Result.Failure<PayrollLoan>(error);

        // TODO: this event could be created inside the Aggregate
        var @event = new PayrollLoanCreated(payrollLoan.Id, payrollLoan.Amount, payrollLoan.InterestRate, payrollLoan.NumberOfInstallments);

        await _repository.AppendEventAsync(@event, cancellationToken);
        
        return payrollLoan;
        
        // try
        // {
        //     var @event = new Event
        //     {
        //         Id = Guid.NewGuid().ToString(),
        //         Type = "PayrollLoanCreated",
        //         Data = request.ToString()
        //     };
        //     var eventCreated = await _repository.AppendEventAsync(@event, cancellationToken);
        //     
        //     return eventCreated;
        // }
        // catch (Exception ex)
        // {
        //     return Result.Failure<Event>($"Failed to append payroll loan event: {ex.Message}");
        // }
    }
}
