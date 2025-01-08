using FinanceSimplify.Exceptions;

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
    public Accounts? Account { get; set; }

    public void Validate()
    {
        if (!Enum.IsDefined(typeof(TransactionType), Type))
        {
            throw new FinanceInternalErrorException("Tipo inválido");
        }

        if (!Enum.IsDefined(typeof(TransactionCategory), Category))
        {
            throw new FinanceInternalErrorException("Categoria inválida");
        }

        if (Value <= 0)
        {
            throw new FinanceInternalErrorException("Valor inválido");
        }
    }

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
    Water,
    Tax
}
