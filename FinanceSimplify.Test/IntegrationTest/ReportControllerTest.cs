using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Controllers;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Report;
using FinanceSimplify.Test.Builder;
using FinanceSimplify.Test.IntegrationTesting.Context;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace FinanceSimplify.Test.IntegrationTest;

public class ReportControllerTest
{
    private readonly ContextFinanceTest _contextTest;
    private readonly TransactionBuilder _transactionBuilder;

    public ReportControllerTest()
    {
        _contextTest = new ContextFinanceTest();
        _transactionBuilder = new TransactionBuilder();
    }

    [Theory]
    [MemberData(nameof(GetTransactionFilters))]
    public async Task GetCategoryReportsWithFilter_ShouldReturnCorrectValues(CategoryFilterReport filter)
    {
        // Arrange
        var context = _contextTest.CreateContext();
        var transactionController = CreateReportController(context);
        var data = _transactionBuilder.BuildRandom(20);
        await AddTransactionDatabase(context, data);

        var expected = data
            .Where(t => (t.DateCreated >= filter.DateStart) &&
                (t.DateCreated <= filter.DateFinish) &&
                (filter.AccountId == null || t.AccountId == filter.AccountId) &&
                (filter.TransactionType == null || t.Type == filter.TransactionType))
            .GroupBy(d => d.Category).Select(group => new CategoryReport
            {
                Category = group.Key,
                Value = group.Sum(t => t.Type == TransactionType.Income ? -t.Value : t.Value)
            }).ToList();

        var expectedTotal = expected.Sum(e => e.Value);

        //Act
        var result = await transactionController.GetCategoryGeneralReport(filter);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);
        var response = Assert.IsAssignableFrom<CategoryGeneralReportDto>(okResult.Value);

        var expectedJson = JsonConvert.SerializeObject(expected.OrderBy(e => e.Category));
        var responseJson = JsonConvert.SerializeObject(response.CategoryReports.OrderBy(e => e.Category));

        Assert.NotNull(response);
        Assert.Equal(expectedJson, responseJson);
        Assert.Equal(response.TotalValue, expectedTotal);
    }

    public static IEnumerable<object[]> GetTransactionFilters()
    {
        yield return new object[]
        {
         new CategoryFilterReport
         {
             AccountId = 1,
             TransactionType = TransactionType.Expense,
         }
        };

        yield return new object[]
        {
         new CategoryFilterReport
         {
             AccountId = 2,
             TransactionType = TransactionType.Income,
             DateStart = DateTime.Now.AddDays(-10),
             DateFinish = DateTime.Now.AddDays(-2),
         }
        };

        yield return new object[]
        {
         new CategoryFilterReport
         {
             AccountId = null,
             TransactionType = null,
         }
        };
    }

    private static ReportController CreateReportController(ContextFinance context)
    {
        var repository = new GenericRepository<Transactions>(context);
        var mapper = new Mock<IMapper>().Object;
        // var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<AccountMapper>()));
        var reportService = new ReportService(repository, mapper);


        return new ReportController(reportService);
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
}
