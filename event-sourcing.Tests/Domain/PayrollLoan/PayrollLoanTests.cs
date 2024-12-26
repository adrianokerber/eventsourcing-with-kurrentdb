using event_sourcing.Domain.PayrollLoan;
using Xunit;

namespace event_sourcing.Tests.Domain.PayrollLoan;

public class PayrollLoanTests
{
    [Fact]
    public void Create_WithValidData_ShouldSucceed()
    {
        // Arrange
        var amount = 1000m;
        var interestRate = 2.5m;
        var numberOfInstallments = 12;

        // Act
        var result = event_sourcing.Domain.PayrollLoan.PayrollLoan.Create(amount, interestRate, numberOfInstallments);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(amount, result.Value.Amount);
        Assert.Equal(interestRate, result.Value.InterestRate);
        Assert.Equal(numberOfInstallments, result.Value.NumberOfInstallments);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.NotEqual(default, result.Value.CreatedAt);
        Assert.Null(result.Value.UpdatedAt);
    }

    [Theory]
    [InlineData(0, 2.5, 12, "Amount must be greater than zero")]
    [InlineData(-100, 2.5, 12, "Amount must be greater than zero")]
    [InlineData(1000, 0, 12, "Interest rate must be greater than zero")]
    [InlineData(1000, -1, 12, "Interest rate must be greater than zero")]
    [InlineData(1000, 2.5, 0, "Number of installments must be greater than zero")]
    [InlineData(1000, 2.5, -6, "Number of installments must be greater than zero")]
    [InlineData(1000, 2.5, 73, "Number of installments cannot exceed 72 months")]
    public void Create_WithInvalidData_ShouldFail(decimal amount, decimal interestRate, int numberOfInstallments, string expectedError)
    {
        // Act
        var result = event_sourcing.Domain.PayrollLoan.PayrollLoan.Create(amount, interestRate, numberOfInstallments);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }
}
