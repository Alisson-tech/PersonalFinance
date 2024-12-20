using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Transaction;

public class TransactionDto
{
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public decimal Value { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public required string AccountName { get; set; } = string.Empty;
}

public class TransactionCreate
{
    public int AccountId { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public decimal Value { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class TransactionFilter
{
    public int? AccountId { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionCategory? Category { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateFinish { get; set; }
    public string? Description { get; set; }
}
