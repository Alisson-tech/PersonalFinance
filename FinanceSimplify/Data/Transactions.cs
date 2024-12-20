namespace FinanceSimplify.Data;

public class Transactions : BaseEntity
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public TransactionType Type { get; set; }
    public TransactionCategory Category { get; set; }
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public Accounts Account { get; set; } = new();
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
