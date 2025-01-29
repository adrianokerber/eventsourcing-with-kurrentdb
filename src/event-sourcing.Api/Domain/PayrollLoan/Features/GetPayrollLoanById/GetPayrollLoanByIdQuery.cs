using CSharpFunctionalExtensions;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.GetPayrollLoanById;

public sealed record GetPayrollLoanByIdQuery : IRequest<Result<PayrollLoan>>
{
    public Guid PayrollLoanId { get; }

    private GetPayrollLoanByIdQuery(Guid payrollLoanId)
    {
        PayrollLoanId = payrollLoanId;
    }

    public static Result<GetPayrollLoanByIdQuery> Create(Guid payrollLoanId)
    {
        if (payrollLoanId == Guid.Empty)
            return Result.Failure<GetPayrollLoanByIdQuery>("Invalid payroll loan id");
        return Result.Success(new GetPayrollLoanByIdQuery(payrollLoanId));
    }
}

public sealed class GetPayrollLoanByIdQueryHandler(PayrollLoansRepository repository)
    : IRequestHandler<GetPayrollLoanByIdQuery, Result<PayrollLoan>>
{
    public async Task<Result<PayrollLoan>> Handle(GetPayrollLoanByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var payrollLoan = await repository.GetPayrollLoanByIdAsync(query.PayrollLoanId, cancellationToken);
            if (payrollLoan.HasNoValue)
                return Result.Failure<PayrollLoan>("Payroll loan not found");
            return payrollLoan.Value;
        }
        catch (Exception ex)
        {
            return Result.Failure<PayrollLoan>($"Failed to retrieve payroll loan with an error: {ex.Message}");
        }
    }
}