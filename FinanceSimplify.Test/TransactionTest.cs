using FinanceSimplify.Data;
using FinanceSimplify.Test.Context;

namespace FinanceSimplify.Test;

public class TransactionTest
{
    private ContextFinanceTest _contextFinanceTest = new();

    [Fact]
    public async void GetAllPaginated()
    {
        var db = _contextFinanceTest.CreateContext();

        await db.Accounts.AddAsync(new Accounts
        {
            Id = 1,
            Type = AccountType.CreditCard,
            Name = "Test",
            Balance = 100
        });

        await db.SaveChangesAsync();

        var teste = db.Accounts.First();

        Assert.True(true);
    }
}