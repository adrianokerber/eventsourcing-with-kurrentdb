using CSharpFunctionalExtensions;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.GetAllPayrollLoans;

public sealed record GetAllPayrollLoansQuery : IRequest<Result<IEnumerable<PayrollLoan>>>
{
    private GetAllPayrollLoansQuery() { }

    public static Result<GetAllPayrollLoansQuery> Create()
    {
        return Result.Success(new GetAllPayrollLoansQuery());
    }
}

public sealed class GetAllPayrollLoansQueryHandler(PayrollLoansRepository repository)
    : IRequestHandler<GetAllPayrollLoansQuery, Result<IEnumerable<PayrollLoan>>>
{
    public async Task<Result<IEnumerable<PayrollLoan>>> Handle(GetAllPayrollLoansQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var payrollLoans = await repository.GetAllPayrollLoansAsync(cancellationToken);
            return Result.Success<IEnumerable<PayrollLoan>>(payrollLoans);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PayrollLoan>>($"Failed to retrieve PayrollLoan list with message: {ex.Message}");
        }
    }
}
