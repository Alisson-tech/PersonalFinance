using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Account;
using FinanceSimplify.Test.Builder;
using FinanceSimplify.Test.Context;

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
        var accountService = CreateAccountService(context);
        int quantityAccountCreate = 5;
        AccountType filterDebitCard = AccountType.DebitCard;

        var datafilterDebitCard = _accountBuilder.WithDefaults(type: filterDebitCard).Build(quantityAccountCreate);
        var datafilterCreditCard = _accountBuilder.WithDefaults(id: quantityAccountCreate + 1, type: AccountType.CreditCard).Build(quantityAccountCreate);
        var data = datafilterDebitCard.Concat(datafilterCreditCard).ToList();
        await AddAccountDatabase(context, data);

        //Act
        var accountsFilter = new AccountsFilter() { Type = filterDebitCard };
        var result = await accountService!.GetAccountList(accountsFilter, new PaginatedFilter());

        //assert
        Assert.NotNull(result);
        Assert.Equal(quantityAccountCreate, result.TotalItems);
        Assert.All(result.Items, account => Assert.Equal(filterDebitCard, account.Type));
    }

    [Fact]
    public async Task GetAccountById_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        int id = 1;
        string name = "Account Test id";
        var data = _accountBuilder.WithDefaults(id: id, name: name).Build(1);
        await AddAccountDatabase(context, data);

        //Act
        var result = await accountService!.GetAccount(id);

        //assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
    }


    [Fact]
    public async Task GetAccountInvalidId_ShouldReturnFinanceException()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        int id = 100;
        var data = _accountBuilder.Build(10);
        await AddAccountDatabase(context, data);

        //Act & assert
        await Assert.ThrowsAsync<FinanceNotFoundException>(async () => await accountService.GetAccount(id));
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        var accountCreate = new AccountCreate() { Name = "Account Test", Type = AccountType.CreditCard, Balance = 1000.00M };

        //Act
        var result = await accountService.CreateAccount(accountCreate);

        //assert
        Assert.NotNull(result);
        Assert.Equal(accountCreate.Name, result.Name);
        Assert.Equal(accountCreate.Type, result.Type);
        Assert.Equal(accountCreate.Balance, result.Balance);
    }

    [Fact]
    public async Task CreateAccountInvalidType_ShouldReturnFinanceException()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        var accountCreate = new AccountCreate() { Name = "Account Test", Type = (AccountType)10, Balance = 1000.00M };

        //Act & assert
        await Assert.ThrowsAsync<FinanceInternalErrorException>(async () => await accountService.CreateAccount(accountCreate));
    }

    [Fact]
    public async Task UpdateAccount_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        int id = 1;
        var data = _accountBuilder.WithDefaults(id: id).Build(1);
        await AddAccountDatabase(context, data);
        var accountCreate = new AccountCreate() { Name = "Account Test Update", Type = AccountType.CreditCard, Balance = 1000.00M };

        //Act
        var result = await accountService.UpdateAccount(id, accountCreate);

        //assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(accountCreate.Name, result.Name);
        Assert.Equal(accountCreate.Type, result.Type);
        Assert.Equal(accountCreate.Balance, result.Balance);
    }

    [Theory]
    [InlineData(100, AccountType.DebitCard, typeof(FinanceNotFoundException))]
    [InlineData(1, (AccountType)10, typeof(FinanceInternalErrorException))]
    public async Task UpdateAccountInvalidId_ShouldReturnFinanceException(int id, AccountType type, Type exceptionType)
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        var data = _accountBuilder.Build(10);
        await AddAccountDatabase(context, data);
        var accountCreate = new AccountCreate() { Name = "Account Test Update", Type = type, Balance = 1000.00M };

        //Act & assert
        await Assert.ThrowsAsync(exceptionType, async () => await accountService.UpdateAccount(id, accountCreate));
    }

    [Fact]
    public async Task DeleteAccount_ShouldReturnCorrectData()
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var accountService = CreateAccountService(context);
        int id = 1;
        var data = _accountBuilder.WithDefaults(id: id).Build(1);
        await AddAccountDatabase(context, data);

        //Act
        await accountService.DeleteAccount(id);

        //assert
        var account = await context.Accounts.FindAsync(id);
        Assert.NotNull(account);
        Assert.NotNull(account.DateDeleted);
    }

    private static async Task AddAccountDatabase(ContextFinance context, List<Accounts> listAccount)
    {
        await context.Accounts.AddRangeAsync(listAccount);
        await context.SaveChangesAsync();
    }

    private static AccountService CreateAccountService(ContextFinance context)
    {
        var repository = new GenericRepository<Accounts>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AccountMapper>()));

        return new AccountService(repository, mapper);
    }
}
