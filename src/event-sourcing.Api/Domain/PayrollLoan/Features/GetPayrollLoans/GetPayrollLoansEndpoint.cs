using event_sourcing.Domain.PayrollLoan.Events;

namespace event_sourcing.Domain.PayrollLoan.Features.GetPayrollLoans;

using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for retrieving payroll loan events
/// </summary>
[ApiController]
[Route("api/payroll-loans")]
public class GetPayrollLoansEndpoint(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets all payroll loan events
    /// </summary>
    /// <returns>List of events</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        var command = GetPayrollLoansCommand.Create();
        if (command.IsFailure)
            return BadRequest(command.Error);

        var result = await mediator.Send(command.Value, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
