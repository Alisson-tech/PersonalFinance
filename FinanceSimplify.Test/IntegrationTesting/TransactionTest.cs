using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Transaction;
using FinanceSimplify.Test.Context;

namespace FinanceSimplify.Test.IntegrationTesting;

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



    private static async Task AddTransactionDatabase(ContextFinance context, List<Transactions> listAccount)
    {
        await context.Transactions.AddRangeAsync(listAccount);
        await context.SaveChangesAsync();
    }

    private static TransactionService CreateTransactionService(ContextFinance context)
    {
        var repository = new GenericRepository<Transactions>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TransactionMapper>()));

        return new TransactionService(repository, mapper);
    }
}