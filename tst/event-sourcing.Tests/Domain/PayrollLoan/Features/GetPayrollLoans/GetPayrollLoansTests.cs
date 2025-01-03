using event_sourcing.Domain.PayrollLoan;
using event_sourcing.Domain.PayrollLoan.Events;
using event_sourcing.Domain.PayrollLoan.Features.GetPayrollLoans;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace event_sourcing.Tests.Domain.PayrollLoan.Features.GetPayrollLoans;

public class GetPayrollLoansTests
{
    private readonly PayrollLoansRepository _repository;
    private readonly GetPayrollLoansCommandHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public GetPayrollLoansTests()
    {
        _repository = Substitute.For<PayrollLoansRepository>();
        _handler = new GetPayrollLoansCommandHandler(_repository);
        _cancellationToken = CancellationToken.None;
    }

    // [Fact]
    // public async Task Handle_ShouldReturnEvents_WhenRepositorySucceeds()
    // {
    //     // Arrange
    //     var expectedEvents = new List<Event>
    //     {
    //         new() { Id = "1", Type = "Created", Data = "data1", CreatedAtUtc = DateTime.UtcNow },
    //         new() { Id = "2", Type = "Updated", Data = "data2", CreatedAtUtc = DateTime.UtcNow }
    //     };
    //
    //     _repository
    //         .GetEventsAsync(_cancellationToken)
    //         .Returns(expectedEvents);
    //
    //     var command = GetPayrollLoansCommand.Create();
    //
    //     // Act
    //     var result = await _handler.Handle(command.Value, _cancellationToken);
    //
    //     // Assert
    //     Assert.True(result.IsSuccess);
    //     Assert.Equal(expectedEvents, result.Value);
    //     await _repository
    //         .Received(1)
    //         .GetEventsAsync(_cancellationToken);
    // }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryThrows()
    {
        // Arrange
        var expectedException = new Exception("Test exception");
        _repository
            .GetEventsAsync(_cancellationToken)
            .Throws(expectedException);

        var command = GetPayrollLoansCommand.Create();

        // Act
        var result = await _handler.Handle(command.Value, _cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains(expectedException.Message, result.Error);
        await _repository
            .Received(1)
            .GetEventsAsync(_cancellationToken);
    }

    [Fact]
    public void Create_ShouldReturnSuccessfulCommand()
    {
        // Act
        var result = GetPayrollLoansCommand.Create();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
