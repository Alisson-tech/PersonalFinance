namespace FinanceSimplify.Services.Report;

public interface IReportService
{
    public Task<CategoryGeneralReportDto> GetCategoryGeneralReport(CategoryFilterReport filter);
    public Task<List<CategoryPercentageReportDto>> GetCategoryPercentage(CategoryFilterReport filter);
}
