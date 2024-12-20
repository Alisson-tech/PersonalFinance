namespace FinanceSimplify.Data;

public class Accounts : BaseEntity
{
    public int Id { get; set; }
    public AccountType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public DateTime? DateDeleted { get; set; }
}

public enum AccountType
{
    CreditCard,
    DebitCard,
    Pix,
    FoodVoucher,
    savings
}
