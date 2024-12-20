using FinanceSimplify.Data;

namespace FinanceSimplify.Test.Builder;

public class TransactionBuilder
{
    private int _id = 1;
    private int _accountId = 1;
    private TransactionType _type;
    private TransactionCategory _category;
    private decimal _value = 100.00M;
    private DateTime _date = DateTime.Now;
    private string _description = "Default Transaction";
    public TransactionBuilder WithDefaults(int? id = null, int? accountId = null, TransactionType type = TransactionType.Expense, TransactionCategory category = TransactionCategory.Debit, decimal? value = null, DateTime? date = null, string description = null)
    {
        _id = id ?? _id;
        _accountId = accountId ?? _accountId;
        _type = type;
        _category = category;
        _value = value ?? _value;
        _description = description ?? _description;
        _date = date ?? _date;
        return this;
    }

    public List<Transactions> Build(int quantity)
    {
        var transactionsList = new List<Transactions>();

        for (int i = 0; i < quantity; i++)
        {
            transactionsList.Add(new Transactions
            {
                Id = _id + i,
                AccountId = _accountId,
                Type = _type,
                Category = _category,
                Value = _value,
                Description = _description,
                Date = _date,
            });
        }

        return transactionsList;
    }
}
