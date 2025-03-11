using FinanceSimplify.Exceptions;
using System.ComponentModel.DataAnnotations;

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
    [Display(Name = "Despesas")]
    Expense,
    [Display(Name = "Receita")]
    Income
}

public enum TransactionCategory
{
    [Display(Name = "Salário")]
    Salary,

    [Display(Name = "Débito")]
    Debit,

    [Display(Name = "iFood")]
    IFood,

    [Display(Name = "Casa")]
    Home,

    [Display(Name = "Uber")]
    Uber,

    [Display(Name = "Eletricidade")]
    Electricity,

    [Display(Name = "Água")]
    Water,

    [Display(Name = "Imposto")]
    Tax
}
