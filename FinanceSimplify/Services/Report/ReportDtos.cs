using FinanceSimplify.Data;
using FinanceSimplify.Infrastructure;

namespace FinanceSimplify.Services.Report;

public class CategoryReport
{
    public TransactionCategory Category { get; set; }
    public TransactionType TransactionType { get; set; }
    public decimal Value { get; set; }
}

public class CategoryPercentageReport
{
    public TransactionType CategoryType { get; set; }
    public decimal Percentage { get; set; }
}

public class CategoryFilterReport : BaseFilter
{
    public TransactionType TransactionType { get; set; }
    public int AccountId { get; set; }
}