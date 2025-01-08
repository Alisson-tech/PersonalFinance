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

public class TransactionControllerTest
{
    private readonly ContextFinanceTest _contextTest;
    private readonly TransactionBuilder _transactionBuilder;

    public TransactionControllerTest()
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
            .CreateDefault(accountId: filter.AccountId, type: filter.Type, category: filter.Category, date: filter.DateStart, description: filter.Description)
            .Build(quantityTransactionCreateFilter);
        var dataGeral = _transactionBuilder.CreateDefault(id: quantityTransactionCreateFilter + 1).Build(quantityTransactionCreateGeral);
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
        var data = _transactionBuilder.CreateDefault(id: id, description: description).Build(1);
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

    [Fact]
    public async Task CreateTransactionIncome_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);

        var accountId = 1;
        var type = TransactionType.Income;
        var category = TransactionCategory.Salary;
        var date = DateTime.Now;
        var description = "Recebimento de salário";
        var value = 51.890M;

        await AddTransactionDatabase(context, new());
        var account = await context.Accounts
            .FirstAsync(a => a.Id == accountId);
        var accountBalance = account.Balance + value;

        //Act
        var result = await transactionController.Create(new TransactionCreate
        { AccountId = accountId, Category = category, Type = type, Date = date, Description = description, Value = value });

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<TransactionDto>(okResult.Value);
        Assert.NotNull(response);
        Assert.Equal(account.Name, response.AccountName);
        Assert.Equal(description, response.Description);
        Assert.Equal(type, response.Type);
        Assert.Equal(category, response.Category);
        Assert.Equal(value, response.Value);
        Assert.Equal(date, response.Date);
        Assert.Equal(accountBalance, account.Balance);
    }

    [Fact]
    public async Task CreateTransactionExpensive_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);

        var accountId = 1;
        var type = TransactionType.Expense;
        var category = TransactionCategory.Debit;
        var date = DateTime.Now;
        var description = "Recebimento de salário";
        var value = 51.890M;

        await AddTransactionDatabase(context, new());
        var account = await context.Accounts
            .FirstAsync(a => a.Id == accountId);
        var accountBalance = account.Balance - value;

        //Act
        var result = await transactionController.Create(new TransactionCreate
        { AccountId = accountId, Category = category, Type = type, Date = date, Description = description, Value = value });

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<TransactionDto>(okResult.Value);
        Assert.NotNull(response);
        Assert.Equal(account.Name, response.AccountName);
        Assert.Equal(description, response.Description);
        Assert.Equal(type, response.Type);
        Assert.Equal(category, response.Category);
        Assert.Equal(value, response.Value);
        Assert.Equal(date, response.Date);
        Assert.Equal(accountBalance, account.Balance);
    }

    [Theory]
    [InlineData(1, (TransactionType)100, (TransactionCategory)1, typeof(BadRequestObjectResult))]
    [InlineData(1, (TransactionType)1, (TransactionCategory)100, typeof(BadRequestObjectResult))]
    [InlineData(100, (TransactionType)1, (TransactionCategory)1, typeof(NotFoundObjectResult))]
    public async Task CreateTransactionInvalid_ShouldReturnStatusCode(int accountId, TransactionType transactionType, TransactionCategory transactionCategory, Type requestObject)
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);

        var transactionCreate = new TransactionCreate() { AccountId = accountId, Type = transactionType, Category = transactionCategory, Value = 100.00M, Date = DateTime.Now, Description = "Transaction Test" };

        //Act & assert
        var result = await transactionController.Create(transactionCreate);
        Assert.IsType(requestObject, result.Result);
    }

    [Fact]
    public async Task DeleteTransactionIncome_ShouldReturnOk()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);

        int id = 1;
        var accountId = 1;
        decimal value = 85.45M;
        var type = TransactionType.Income;

        var data = _transactionBuilder.CreateDefault(id: id, accountId: accountId, value: value, type: type).Build(1);
        await AddTransactionDatabase(context, data);
        var account = await context.Accounts
            .FirstAsync(a => a.Id == accountId);
        var accountBalance = account.Balance + value;

        //Act
        var result = await transactionController.Delete(id);

        //assert
        var transaction = await context.Transactions.FindAsync(id);

        Assert.Null(transaction);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(accountBalance, account.Balance);
    }

    [Fact]
    public async Task DeleteTransactionExpensive_ShouldReturnOk()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);

        int id = 1;
        var accountId = 1;
        decimal value = 85.45M;
        var type = TransactionType.Expense;

        var data = _transactionBuilder.CreateDefault(id: id, accountId: accountId, value: value, type: type).Build(1);
        await AddTransactionDatabase(context, data);
        var account = await context.Accounts
            .FirstAsync(a => a.Id == accountId);
        var accountBalance = account.Balance - value;

        //Act
        var result = await transactionController.Delete(id);

        //assert
        var transaction = await context.Transactions.FindAsync(id);

        Assert.Null(transaction);
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(accountBalance, account.Balance);
    }

    [Fact]
    public async Task DeleteTransactioninvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateTransactionController(context);
        int id = 100;

        var data = _transactionBuilder.Build(1);
        await AddTransactionDatabase(context, data);

        //Act
        var result = await transactionController.Delete(id);

        //assert
        var transaction = await context.Transactions.FindAsync(id);

        Assert.IsType<NotFoundObjectResult>(result);
    }


    private static async Task AddTransactionDatabase(ContextFinance context, List<Transactions> listTransaction)
    {
        var AccounntBuilder = new AccountsBuilder();
        var listtransaction = AccounntBuilder.CreateDefault().Build(3);

        await context.Accounts.AddRangeAsync(listtransaction);
        await context.SaveChangesAsync();

        await context.Transactions.AddRangeAsync(listTransaction);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();
    }

    private static TransactionController CreateTransactionController(ContextFinance context)
    {
        var accountRepository = new GenericRepository<Transactions>(context);
        var transactionRepository = new GenericRepository<Accounts>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<TransactionMapper>()));
        var service = new TransactionService(accountRepository, transactionRepository, mapper);

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

    public static IEnumerable<object[]> GetTransactionCreate()
    {
        yield return new object[]
        {
            new TransactionCreate
            {
                AccountId = 1,
                Type = TransactionType.Income,
                Category = TransactionCategory.Salary,
                Date = DateTime.Now,
                Description = "Recebimento de salário",
                Value = 51.890M
            },
            new TransactionCreate
            {
                AccountId = 2,
                Type = TransactionType.Expense,
                Category = TransactionCategory.Debit,
                Date = DateTime.Now,
                Description = "Pix para Victor",
                Value = 35.670M
            }
        };
    }
}