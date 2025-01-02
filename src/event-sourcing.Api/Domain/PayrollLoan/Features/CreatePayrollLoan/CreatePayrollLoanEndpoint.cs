using event_sourcing.Domain.PayrollLoan.Events;
using MediatR;

namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for creating payroll loan events
/// </summary>
[ApiController]
[Route("api/payroll-loans")]
public class CreatePayrollLoanEndpoint(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new payroll loan event
    /// </summary>
    /// <param name="request">The request data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created event</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEvent([FromBody] Request request, CancellationToken cancellationToken)
    {
        var command = CreatePayrollLoanCommand.Create(request.Amount, request.InterestRate, request.TermMonths);
        if (command.IsFailure)
            return BadRequest(command.Error);
        
        var result = await mediator.Send(command.Value, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction("GetEvents", "GetPayrollLoansEndpoint", new { streamName = "sample-stream" }, result.Value);
    }
}

public record Request(decimal Amount, decimal InterestRate, int TermMonths);