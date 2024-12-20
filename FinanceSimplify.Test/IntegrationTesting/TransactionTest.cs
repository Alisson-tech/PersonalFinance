using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Data;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Transaction;
using FinanceSimplify.Test.Builder;
using FinanceSimplify.Test.Context;
using Microsoft.EntityFrameworkCore;

namespace FinanceSimplify.Test.IntegrationTesting;

public class TransactionTest
{
    private readonly ContextFinanceTest _contextTest;
    private readonly TransactionBuilder _transactionBuilder;

    public TransactionTest()
    {
        _contextTest = new ContextFinanceTest();
        _transactionBuilder = new TransactionBuilder();
    }

    [Theory]
    [MemberData(nameof(GetTransactionFilters))]
    public async Task GetTransactionsWithFilter_ShouldReturnCorrectNumberOfTransaction(TransactionFilter filter)
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateTransactionService(context);
        int quantityTransactionCreateFilter = 5;
        int quantityTransactionCreateGeral = 10;


        var datafilter = _transactionBuilder
            .WithDefaults(accountId: filter.AccountId, type: filter.Type, category: filter.Category, date: filter.DateStart, description: filter.Description)
            .Build(quantityTransactionCreateFilter);
        var dataGeral = _transactionBuilder.WithDefaults(id: quantityTransactionCreateFilter + 1).Build(quantityTransactionCreateGeral);
        var data = datafilter.Concat(dataGeral).ToList();
        await AddTransactionDatabase(context, data);
        var teste = context
            .Transactions.ToList();

        //Act
        var result = await accountService!.GetTransactionList(filter, new PaginatedFilter());

        //assert
        var totalItems = await context.Transactions.CountAsync();
        Assert.NotNull(result);
        Assert.Equal(quantityTransactionCreateFilter, result.TotalItems);
        Assert.Equal(quantityTransactionCreateFilter + quantityTransactionCreateGeral, totalItems);
    }

    private static async Task AddTransactionDatabase(ContextFinance context, List<Transactions> listTransaction)
    {
        var AccounntBuilder = new AccountsBuilder();
        var listAccount = AccounntBuilder.WithDefaults().Build(3);

        await context.Accounts.AddRangeAsync(listAccount);
        await context.SaveChangesAsync();

        await context.Transactions.AddRangeAsync(listTransaction);
        await context.SaveChangesAsync();
    }

    private static TransactionService CreateTransactionService(ContextFinance context)
    {
        var repository = new GenericRepository<Transactions>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TransactionMapper>()));

        return new TransactionService(repository, mapper);
    }

    public static IEnumerable<object[]> GetTransactionFilters()
    {
        yield return new object[]
        {
            new TransactionFilter
            {
                AccountId = 3,
                Type = TransactionType.Income,
                Category = TransactionCategory.Salary,
                DateStart = null,
                DateFinish = null,
                Description = "Recebimento de salário"
            }
        };

        yield return new object[]
        {
            new TransactionFilter
            {
                AccountId = 2,
                Type = null,
                Category = null,
                DateStart = DateTime.Now.AddDays(-10),
                DateFinish = DateTime.Now.AddDays(-2),
                Description = null
            }
        };

        yield return new object[]
        {
            new TransactionFilter
            {
                AccountId = null,
                Type = TransactionType.Expense,
                Category = null,
                DateStart = DateTime.Now.AddDays(-10),
                DateFinish = DateTime.Now.AddDays(-2),
                Description = null
            }
        };

        yield return new object[]
        {
            new TransactionFilter
            {
                AccountId = 3,
                Type = TransactionType.Expense,
                Category = TransactionCategory.Debit,
                DateStart = DateTime.Now.AddDays(-10),
                DateFinish = DateTime.Now.AddDays(-2),
                Description = "Pix para Victor"
            }
        };
    }
}