using FinanceSimplify.Data;

namespace FinanceSimplify.Test.Builder;

public class AccountsBuilder
{
    private int _id = 1;
    private AccountType _type = AccountType.CreditCard;
    private string _name = "Default Account";
    private decimal _balance = 1000.00M;
    private DateTime? _dateDeleted = null;


    public AccountsBuilder CreateDefault(int? id = null, AccountType? type = null, string name = null, decimal? balance = null, DateTime? dateDeleted = null)
    {
        _id = id ?? _id;
        _type = type ?? AccountType.CreditCard;
        _name = name ?? "Default Account";
        _balance = balance ?? 1000.00M;
        _dateDeleted = dateDeleted ?? null;

        return this;
    }

    public List<Accounts> Build(int quantity)
    {
        var accountsList = new List<Accounts>();

        for (int i = 0; i < quantity; i++)
        {
            accountsList.Add(new Accounts
            {
                Id = _id + i,
                Type = _type,
                Name = _name,
                Balance = _balance,
                DateDeleted = _dateDeleted
            });
        }

        return accountsList;
    }
}