using event_sourcing.Domain.PayrollLoan.Events;
using event_sourcing.Domain.Shared;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for creating payroll loans
/// </summary>
[ApiController]
[Route("api/payroll-loans")]
public class CreatePayrollLoanEndpoint(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new payroll loan
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created payroll-loan</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PayrollLoan), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEvent([FromBody] Request request, CancellationToken cancellationToken)
    {
        var command = CreatePayrollLoanCommand.Create(request.Amount, request.InterestRate, request.NumberOfInstallments);
        if (command.IsFailure)
            return BadRequest(command.Error);
        
        var result = await mediator.Send(command.Value, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction("GetPayrollLoanById", "GetPayrollLoanByIdEndpoint", new { id = result.Value.Id }, result.Value);
    }
}

public record Request(decimal Amount, decimal InterestRate, int NumberOfInstallments);