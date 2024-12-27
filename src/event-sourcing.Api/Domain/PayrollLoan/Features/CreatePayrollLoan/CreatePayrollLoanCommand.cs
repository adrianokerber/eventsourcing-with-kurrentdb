using CSharpFunctionalExtensions;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

public sealed record CreatePayrollLoanCommand : IRequest<Result<Event>>
{
    public Event Event { get; init; }

    private CreatePayrollLoanCommand(Event @event)
    {
        Event = @event;
    }

    // TODO: the command must have its values instead of a 
    public static Result<CreatePayrollLoanCommand> Create(Event @event)
    {
        if (@event is null || @event.Data is null)
            return Result.Failure<CreatePayrollLoanCommand>("Invalid event");

        return new CreatePayrollLoanCommand(@event);
    }
}

public sealed class CreatePayrollLoanCommandHandler : IRequestHandler<CreatePayrollLoanCommand, Result<Event>>
{
    private readonly PayrollLoansRepository _repository;

    public CreatePayrollLoanCommandHandler(PayrollLoansRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Event>> Handle(CreatePayrollLoanCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var eventCreated = await _repository.AppendEventAsync("sample-stream", request.Event, cancellationToken);
            
            return eventCreated;
        }
        catch (Exception ex)
        {
            return Result.Failure<Event>($"Failed to append payroll loan event: {ex.Message}");
        }
    }
}
