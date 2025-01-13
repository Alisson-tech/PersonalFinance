using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Services.Report;

namespace FinanceSimplify.Test.UnitTest;

public class ReportTest
{
    [Fact]
    public void ValidateCategoryFilterTypeNotNull_ShouldPassMethod()
    {
        // Arrange
        var categoryFilter = new CategoryFilterReport()
        {
            TransactionType = TransactionType.Expense,
            AccountId = 1
        };

        // Act
        var exception = Record.Exception(() => categoryFilter.ValidateTransactionType());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateCategoryFilterTypeNull_ShouldReturnFinanceException()
    {
        // Arrange
        var categoryFilter = new CategoryFilterReport()
        {
            TransactionType = null,
            AccountId = 1
        };

        // Act
        var exception = Record.Exception(() => categoryFilter.ValidateTransactionType());

        // Assert
        Assert.IsType<FinanceInternalErrorException>(exception);
    }
}
