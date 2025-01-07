using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Controllers;
using FinanceSimplify.Data;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Transaction;
using FinanceSimplify.Test.Builder;
using FinanceSimplify.Test.IntegrationTesting.Context;
using Microsoft.AspNetCore.Mvc;
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
        var transactionController = CreateTransactionController(context);
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
        var result = await transactionController!.GetAll(filter, new PaginatedFilter());

        //assert
        var totalItems = await context.Transactions.CountAsync();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<PaginatedList<TransactionDto>>(okResult.Value);
        Assert.NotNull(response);
        Assert.Equal(quantityTransactionCreateFilter, response.TotalItems);
        Assert.Equal(quantityTransactionCreateFilter + quantityTransactionCreateGeral, totalItems);
    }

    [Fact]
    public async Task GetTransactionById_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);
        int id = 1;
        string description = "Transaction Test id";
        var data = _transactionBuilder.WithDefaults(id: id, description: description).Build(1);
        await AddTransactionDatabase(context, data);

        //Act
        var result = await transactionController!.GetId(id);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<TransactionDto>(okResult.Value);
        Assert.NotNull(response);
        Assert.Equal(id, response.Id);
        Assert.Equal(description, response.Description);
    }

    [Fact]
    public async Task GetTransactionInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);
        int id = 100;
        var data = _transactionBuilder.Build(10);
        await AddTransactionDatabase(context, data);

        //Act & assert
        var result = await transactionController!.GetId(id);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    private static async Task AddTransactionDatabase(ContextFinance context, List<Transactions> listTransaction)
    {
        var AccounntBuilder = new AccountsBuilder();
        var listAccount = AccounntBuilder.CreateDefault().Build(3);

        await context.Accounts.AddRangeAsync(listAccount);
        await context.SaveChangesAsync();

        await context.Transactions.AddRangeAsync(listTransaction);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();
    }

    private static TransactionController CreateTransactionController(ContextFinance context)
    {
        var repository = new GenericRepository<Transactions>(context);
        var accountRepository = new GenericRepository<Accounts>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TransactionMapper>()));
        var service = new TransactionService(repository, accountRepository, mapper);

        return new TransactionController(service);
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