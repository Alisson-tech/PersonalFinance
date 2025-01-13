namespace FinanceSimplify.Services.Report;

public interface IReportService
{
    public Task<List<CategoryReport>> GetCategoryReport();
    public Task<List<CategoryPercentageReport>> GetCategoryPercentage();
}
