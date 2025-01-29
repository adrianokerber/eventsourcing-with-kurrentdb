using event_sourcing.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace event_sourcing.Domain.PayrollLoan.Features.GetPayrollLoanById;

/// <summary>
/// Endpoint for retrieving payroll loan by ID
/// </summary>
[ApiController]
[Route("api/payroll-loans")]
public class GetPayrollLoanByIdEndpoint(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets payroll by ID
    /// </summary>
    /// <returns>The PayrollLoan</returns>
    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(typeof(IEnumerable<Event>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPayrollLoanById(Guid id, CancellationToken cancellationToken)
    {
        var query = GetPayrollLoanByIdQuery.Create(id);
        if (query.IsFailure)
            return StatusCode(StatusCodes.Status500InternalServerError, query.Error);

        var result = await mediator.Send(query.Value, cancellationToken);
        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}
