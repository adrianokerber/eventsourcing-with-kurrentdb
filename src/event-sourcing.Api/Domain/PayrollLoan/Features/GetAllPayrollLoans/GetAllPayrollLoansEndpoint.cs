using event_sourcing.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace event_sourcing.Domain.PayrollLoan.Features.GetAllPayrollLoans;

/// <summary>
/// Endpoint for retrieving payroll loans
/// </summary>
[ApiController]
[Route("api/payroll-loans")]
public class GetAllPayrollLoansEndpoint(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets all payroll loans
    /// </summary>
    /// <returns>List of PayrollLoans</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllPayrollLoans(CancellationToken cancellationToken)
    {
        var query = GetAllPayrollLoansQuery.Create();
        if (query.IsFailure)
            return BadRequest(query.Error);

        var result = await mediator.Send(query.Value, cancellationToken);
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
