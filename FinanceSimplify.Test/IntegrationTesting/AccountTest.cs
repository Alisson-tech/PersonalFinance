using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Controllers;
using FinanceSimplify.Data;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Account;
using FinanceSimplify.Test.Builder;
using FinanceSimplify.Test.IntegrationTesting.Context;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Test.IntegrationTesting;

public class AccountTest
{
    private readonly ContextFinanceTest _contextTest;
    private readonly AccountsBuilder _accountBuilder;

    public AccountTest()
    {
        _contextTest = new ContextFinanceTest();
        _accountBuilder = new AccountsBuilder();
    }

    [Fact]
    public async Task GetAccountsWithFilterDebitCard_ShouldReturnCorrectNumberOfAccounts()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        int quantityAccountCreate = 5;
        AccountType filterDebitCard = AccountType.DebitCard;

        var datafilterDebitCard = _accountBuilder.CreateDefault(type: filterDebitCard).Build(quantityAccountCreate);
        var datafilterCreditCard = _accountBuilder.CreateDefault(id: quantityAccountCreate + 1, type: AccountType.CreditCard).Build(quantityAccountCreate);
        var data = datafilterDebitCard.Concat(datafilterCreditCard).ToList();
        await AddAccountDatabase(context, data);

        //Act
        var accountsFilter = new AccountsFilter() { Type = filterDebitCard };
        var result = await accountController!.GetAll(accountsFilter, new PaginatedFilter());

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<PaginatedList<AccountDto>>(okResult.Value);
        Assert.NotNull(response);
        Assert.Equal(quantityAccountCreate, response.TotalItems);
        Assert.All(response.Items, account => Assert.Equal(filterDebitCard, account.Type));
    }

    [Fact]
    public async Task GetAccountById_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        int id = 1;
        string name = "Account Test id";
        var data = _accountBuilder.CreateDefault(id: id, name: name).Build(1);
        await AddAccountDatabase(context, data);

        //Act
        var result = await accountController!.GetId(id);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<AccountDto>(okResult.Value);
        Assert.NotNull(response);

        Assert.Equal(id, response.Id);
        Assert.Equal(name, response.Name);
    }


    [Fact]
    public async Task GetAccountInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        int id = 100;
        var data = _accountBuilder.Build(10);
        await AddAccountDatabase(context, data);

        //Act & assert
        var result = await accountController.GetId(id);
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        var accountCreate = new AccountCreate() { Name = "Account Test", Type = AccountType.CreditCard, Balance = 1000.00M };

        //Act
        var result = await accountController.Create(accountCreate);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<AccountDto>(okResult.Value);

        Assert.NotNull(response);
        Assert.Equal(accountCreate.Name, response.Name);
        Assert.Equal(accountCreate.Type, response.Type);
        Assert.Equal(accountCreate.Balance, response.Balance);
    }

    [Fact]
    public async Task CreateAccountInvalidType_ShouldReturnBadRequest()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        var accountCreate = new AccountCreate() { Name = "Account Test", Type = (AccountType)10, Balance = 1000.00M };

        //Act & assert
        var result = await accountController.Create(accountCreate);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        int id = 1;
        var data = _accountBuilder.CreateDefault(id: id).Build(1);
        await AddAccountDatabase(context, data);
        var accountCreate = new AccountCreate() { Name = "Account Test Update", Type = AccountType.CreditCard, Balance = 1000.00M };

        //Act
        var result = await accountController.Update(id, accountCreate);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<AccountDto>(okResult.Value);
        Assert.NotNull(response);

        Assert.Equal(accountCreate.Name, response.Name);
        Assert.Equal(accountCreate.Type, response.Type);
        Assert.Equal(accountCreate.Balance, response.Balance);
    }

    [Theory]
    [InlineData(100, AccountType.DebitCard, typeof(NotFoundObjectResult))]
    [InlineData(1, (AccountType)10, typeof(BadRequestObjectResult))]
    public async Task UpdateAccountInvalid_ShouldReturnStatusCode(int id, AccountType accountType, Type requestObject)
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        var data = _accountBuilder.Build(10);
        await AddAccountDatabase(context, data);
        var accountCreate = new AccountCreate() { Name = "Account Test Update", Type = accountType, Balance = 1000.00M };

        //Act & assert
        var result = await accountController.Update(id, accountCreate);
        Assert.IsType(requestObject, result.Result);
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnOk()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountController = CreateAccountController(context);
        int id = 1;
        var data = _accountBuilder.CreateDefault(id: id).Build(1);
        await AddAccountDatabase(context, data);

        //Act
        var result = await accountController.Delete(id);

        //assert
        var account = await context.Accounts.FindAsync(id);
        Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(account);
        Assert.NotNull(account.DateDeleted);
    }

    private static async Task AddAccountDatabase(ContextFinance context, List<Accounts> listAccount)
    {
        await context.Accounts.AddRangeAsync(listAccount);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();
    }

    private static AccountController CreateAccountController(ContextFinance context)
    {
        var repository = new GenericRepository<Accounts>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AccountMapper>()));
        var accountService = new AccountService(repository, mapper);


        return new AccountController(accountService);
    }
}
