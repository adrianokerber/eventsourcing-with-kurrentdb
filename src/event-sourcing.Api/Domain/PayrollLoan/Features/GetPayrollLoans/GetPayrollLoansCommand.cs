namespace event_sourcing.Domain.PayrollLoan.Features.GetPayrollLoans;

using CSharpFunctionalExtensions;
using MediatR;

public sealed record GetPayrollLoansCommand : IRequest<Result<IEnumerable<Event>>>
{
    private GetPayrollLoansCommand() { }

    public static Result<GetPayrollLoansCommand> Create()
    {
        return Result.Success(new GetPayrollLoansCommand());
    }
}

public sealed class GetPayrollLoansCommandHandler : IRequestHandler<GetPayrollLoansCommand, Result<IEnumerable<Event>>>
{
    private readonly PayrollLoansRepository _repository;

    public GetPayrollLoansCommandHandler(PayrollLoansRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Event>>> Handle(GetPayrollLoansCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var events = await _repository.GetEventsAsync(cancellationToken);
            return Result.Success<IEnumerable<Event>>(events);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<Event>>($"Failed to retrieve payroll loan events: {ex.Message}");
        }
    }
}
