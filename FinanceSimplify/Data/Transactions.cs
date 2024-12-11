namespace FinanceSimplify.Data;

public class Transactions : BaseEntity
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public TransactionType Type { get; set; } 
    public TransactionCategory Category { get; set; }
    public decimal Value { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public required Accounts Account { get; set; }
}

public enum TransactionType
{
    Expense,
    Income
}

public enum TransactionCategory
{
    Salary,
    Debit,

    IFood,
    Home,
    Uber,
    Electricity,
    water,
    tax
}
