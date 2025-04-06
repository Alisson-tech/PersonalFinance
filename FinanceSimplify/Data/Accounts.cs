using FinanceSimplify.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace FinanceSimplify.Data;

public class Accounts : BaseEntity
{
    public int Id { get; set; }
    public AccountType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime? DateDeleted { get; set; }

    public void Validate()
    {
        if (!Enum.IsDefined(typeof(AccountType), Type))
        {
            throw new FinanceInternalErrorException("Tipo inválido");
        }
    }

    public void UpdateBalance(decimal value, TransactionType transactionType)
    {
        Balance = transactionType == TransactionType.Income ?
            Balance + value : Balance - value;
    }
}

public enum AccountType
{
    [Display(Name = "Crédito")]
    CreditCard,
    [Display(Name = "Débito")]
    DebitCard,
    [Display(Name = "Pix")]
    Pix,
    [Display(Name = "Voucher Alimentação")]
    FoodVoucher,
    [Display(Name = "Poupança")]
    savings
}
