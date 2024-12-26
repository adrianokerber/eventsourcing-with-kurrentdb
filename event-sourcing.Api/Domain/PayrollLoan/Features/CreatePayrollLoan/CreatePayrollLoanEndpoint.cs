namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoint for creating payroll loan events
/// </summary>
[ApiController]
[Route("api/payroll-loans")]
public class CreatePayrollLoanEndpoint(PayrollLoansRepository eventStoreService) : ControllerBase
{
    /// <summary>
    /// Creates a new payroll loan event
    /// </summary>
    /// <param name="event">The event data</param>
    /// <returns>The created event</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostEvent([FromBody] Event @event)
    {
        await eventStoreService.AppendEventAsync("sample-stream", @event);
        return CreatedAtAction("GetEvents", "GetPayrollLoansEndpoint", new { streamName = "sample-stream" }, @event);
    }
}