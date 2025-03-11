using FinanceSimplify.Data;
using FinanceSimplify.Infrastructure;

namespace FinanceSimplify.Services.Transaction;

public class TransactionDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public required string AccountName { get; set; } = string.Empty;
}

public class TransactionCreate
{
    public int AccountId { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public decimal Value { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class TransactionFilter : BaseFilter
{
    public int? AccountId { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionCategory? Category { get; set; }
    public string? Description { get; set; }
}
