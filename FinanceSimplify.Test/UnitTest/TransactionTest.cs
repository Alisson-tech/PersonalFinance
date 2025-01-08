using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Test.Builder;

namespace FinanceSimplify.Test.UnitTest;

public class TransactionTest
{
    private readonly TransactionBuilder _transactionBuilder;

    public TransactionTest()
    {
        _transactionBuilder = new TransactionBuilder();
    }

    [Theory]
    [InlineData(TransactionType.Income, TransactionCategory.IFood)]
    [InlineData(TransactionType.Expense, TransactionCategory.Water)]
    public void ValidateTransaction_ShouldPassMethod(TransactionType type, TransactionCategory category)
    {
        // Arrange
        var transaction = (_transactionBuilder.CreateDefault(type: type, category: category).Build(1)).First();

        // Act
        var exception = Record.Exception(() => transaction.Validate());

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData((TransactionType)1000, (TransactionCategory)1)]
    [InlineData((TransactionType)1, (TransactionCategory)1000)]
    public void ValidateTransaction_ShouldReturn(TransactionType type, TransactionCategory category)
    {
        // Arrange
        var transaction = (_transactionBuilder.CreateDefault(type: type, category: category).Build(1)).First();

        // Act
        var exception = Record.Exception(() => transaction.Validate());

        // Assert
        Assert.IsType<FinanceInternalErrorException>(exception);
    }
}
