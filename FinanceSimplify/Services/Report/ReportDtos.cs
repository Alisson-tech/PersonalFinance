using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Infrastructure;

namespace FinanceSimplify.Services.Report;

public class CategoryPercentageReportDto
{
    public string Category { get; set; } = String.Empty;
    public decimal Percentage { get; set; }
}

public class CategoryGeneralReportDto
{
    public List<CategoryReport> CategoryReports { get; set; } = new();
    public decimal TotalValue => CategoryReports.Sum(report => report.Value);
}

public class CategoryReport
{
    public string Category { get; set; } = String.Empty;
    public decimal Value { get; set; }
}

public class CategoryFilterReport : BaseFilter
{
    public TransactionType? TransactionType { get; set; }
    public int? AccountId { get; set; }

    public void ValidateTransactionTypeNotNull()
    {
        if (TransactionType == null)
            throw new FinanceInternalErrorException("Tipo de transaçao inválido");
    }
}