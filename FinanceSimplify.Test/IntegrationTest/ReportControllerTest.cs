using FinanceSimplify.Data;
using FinanceSimplify.Services.Report;
using FinanceSimplify.Test.Builder;
using FinanceSimplify.Test.IntegrationTesting.Context;

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

    //[Theory]
    //[MemberData(nameof(GetTransactionFilters))]
    //public async Task GetCategoryReportsWithFilter_ShouldReturnCorrectValues(CategoryFilterReport filter)
    //{
    //    // Arrange
    //    var context = _contextTest.CreateContext();
    //    var transactionController = CreateTransactionController(context);
    //    int quantityTransactionCreateFilter = 5;
    //    int quantityTransactionCreateGeral = 10;


    //    var datafilter = _transactionBuilder
    //        .CreateDefault(accountId: filter.AccountId, type: filter.Type, category: filter.Category, date: filter.DateStart, description: filter.Description)
    //        .Build(quantityTransactionCreateFilter);
    //    var dataGeral = _transactionBuilder.CreateDefault(id: quantityTransactionCreateFilter + 1).Build(quantityTransactionCreateGeral);
    //    var data = datafilter.Concat(dataGeral).ToList();
    //    await AddTransactionDatabase(context, data);
    //    var teste = context
    //        .Transactions.ToList();

    //    //Act
    //    var result = await transactionController!.GetAll(filter, new PaginatedFilter());

    //    //assert
    //    var totalItems = await context.Transactions.CountAsync();
    //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    //    Assert.NotNull(okResult);

    //    var response = Assert.IsAssignableFrom<PaginatedList<TransactionDto>>(okResult.Value);
    //    Assert.NotNull(response);
    //    Assert.Equal(quantityTransactionCreateFilter, response.TotalItems);
    //    Assert.Equal(quantityTransactionCreateFilter + quantityTransactionCreateGeral, totalItems);
    //}

    public static IEnumerable<object[]> GetTransactionFilters()
    {
        yield return new object[]
        {
         new CategoryFilterReport
         {
             AccountId = 3,
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
}
