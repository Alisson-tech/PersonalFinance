using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Test.Builder;

namespace FinanceSimplify.Test.UnitTest;

public class AccountTest
{
    private readonly AccountsBuilder _accountBuilder;

    public AccountTest()
    {
        _accountBuilder = new AccountsBuilder();
    }

    [Fact]
    public void ValidateAccountType_ShouldPassMethod()
    {
        // Arrange
        AccountType accountType = AccountType.DebitCard;
        var account = (_accountBuilder.CreateDefault(type: accountType).Build(1)).First();

        // Act
        var exception = Record.Exception(() => account.Validate());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void ValidateErrorAccountType_ShouldReturnFinanceException()
    {
        // Arrange
        AccountType accountType = (AccountType)10;
        var account = (_accountBuilder.CreateDefault(type: accountType).Build(1)).First();

        // Act
        var exception = Record.Exception(() => account.Validate());

        // Assert
        Assert.IsType<FinanceInternalErrorException>(exception);

    }

    [Fact]
    public void UpdateBalanceIncome_ShouldReturnCorrectBalance()
    {
        // Arrange
        var account = (_accountBuilder.CreateDefault().Build(1)).First();
        decimal transactionValue = 100;
        decimal totalAccountBalance = account.Balance + transactionValue;
        TransactionType type = TransactionType.Income;

        // Act
        account.UpdateBalance(transactionValue, type);

        // Assert
        Assert.Equal(totalAccountBalance, account.Balance);
    }

    [Fact]
    public void UpdateBalanceExpenses_ShouldReturnCorrectBalance()
    {
        // Arrange
        var account = (_accountBuilder.CreateDefault().Build(1)).First();
        decimal transactionValue = 100;
        decimal totalAccountBalance = account.Balance - transactionValue;
        TransactionType type = TransactionType.Expense;

        // Act
        account.UpdateBalance(transactionValue, type);

        // Assert
        Assert.Equal(totalAccountBalance, account.Balance);
    }
}
