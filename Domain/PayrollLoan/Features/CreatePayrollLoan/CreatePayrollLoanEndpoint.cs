namespace event_sourcing.Domain.PayrollLoan.Features.CreatePayrollLoan;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Endpoints for managing payroll loan events
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
        return CreatedAtAction(nameof(GetEvents), new { streamName = "sample-stream" }, @event);
    }

    /// <summary>
    /// Gets all payroll loan events
    /// </summary>
    /// <returns>List of events</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents()
    {
        var events = await eventStoreService.GetEventsAsync("sample-stream");
        return Ok(events);
    }
}