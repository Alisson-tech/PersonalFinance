using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;

namespace FinanceSimplify.Test.UnitTest;

public class UserTest
{
    [Theory]
    [InlineData("", "validEmail@example.com", "ValidPassword123")]
    [InlineData("Valid Name", "", "ValidPassword123")]
    [InlineData("Valid Name", "invalid-email", "ValidPassword123")]
    [InlineData("Valid Name", "validemail@example.com", "")]
    [InlineData("Valid Name", "validemail@example.com", "1234567")]
    public void Validate_ShouldThrowException_WhenInputIsInvalid(
    string name,
    string email,
    string password)
    {
        // Arrange
        var user = new Users
        {
            Name = name,
            Email = email,
            Password = password
        };

        // Act & Assert
        var exception = Record.Exception(() => user.Validate());
        Assert.IsType<FinanceInternalErrorException>(exception);
    }

    [Fact]
    public void Validate_ShouldPass_WhenAllFieldsAreValid()
    {
        // Arrange
        var user = new Users
        {
            Name = "Valid Name",
            Email = "validemail@example.com",
            Password = "ValidPassword123"
        };

        // Act & Assert
        var exception = Record.Exception(() => user.Validate());
        Assert.Null(exception);
    }
}
