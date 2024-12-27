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
    /// <param name="event">The event data</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created event</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEvent([FromBody] Event @event, CancellationToken cancellationToken)
    {
        var command = CreatePayrollLoanCommand.Create(@event);
        if (command.IsFailure)
            return BadRequest(command.Error);
        
        var result = await mediator.Send(command.Value, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        //return Ok(result.Value);
        return CreatedAtAction("GetEvents", "GetPayrollLoansEndpoint", new { streamName = "sample-stream" }, result.Value);
    }
}