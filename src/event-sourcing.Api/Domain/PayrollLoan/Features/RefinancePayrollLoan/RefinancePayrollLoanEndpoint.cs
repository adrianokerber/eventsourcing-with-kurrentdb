using event_sourcing.Domain.PayrollLoan.Events;
using event_sourcing.Domain.PayrollLoan.Features.RefinancePayrollLoan;
using event_sourcing.Domain.Shared;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.RefinancePayrollLoan;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for refinance a payroll loan
/// </summary>
[ApiController]
[Route("api/payroll-loans/")]
public class RefinancePayrollLoanEndpoint(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Refinances a new payroll loan
    /// </summary>
    /// <param name="id">The ID of the resource</param>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The Refinanced payroll-loan</returns>
    [HttpPost]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(PayrollLoan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEvent(Guid id, [FromBody] Request request, CancellationToken cancellationToken)
    {
        var command = RefinancePayrollLoanCommand.Create(id, request.Amount, request.NumberOfInstallments);
        if (command.IsFailure)
            return BadRequest(command.Error);
        
        var result = await mediator.Send(command.Value, cancellationToken);
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}

public record Request(decimal Amount, int NumberOfInstallments);