using event_sourcing.Domain.PayrollLoan.Events;
using event_sourcing.Domain.Shared;

namespace event_sourcing.Domain.PayrollLoan.Features.GetPayrollLoans;

using CSharpFunctionalExtensions;
using MediatR;

public sealed record GetPayrollLoansQuery : IRequest<Result<IEnumerable<PayrollLoan>>>
{
    private GetPayrollLoansQuery() { }

    public static Result<GetPayrollLoansQuery> Create()
    {
        return Result.Success(new GetPayrollLoansQuery());
    }
}

public sealed class GetPayrollLoansQueryHandler : IRequestHandler<GetPayrollLoansQuery, Result<IEnumerable<PayrollLoan>>>
{
    private readonly PayrollLoansRepository _repository;

    public GetPayrollLoansQueryHandler(PayrollLoansRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PayrollLoan>>> Handle(GetPayrollLoansQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payrollLoans = await _repository.GetAllPayrollLoansAsync(cancellationToken);
            return Result.Success<IEnumerable<PayrollLoan>>(payrollLoans);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PayrollLoan>>($"Failed to retrieve payroll loan events: {ex.Message}");
        }
    }
}
