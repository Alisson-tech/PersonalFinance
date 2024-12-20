using FinanceSimplify.Data;

namespace FinanceSimplify.Test.Builder;

public class TransactionBuilder
{
    private int _id;
    private int _accountId;
    private TransactionType _type;
    private TransactionCategory _category;
    private decimal _value;
    private DateTime _date;
    private string _description;
    public TransactionBuilder WithDefaults(int? id = null, int? accountId = null, TransactionType? type = TransactionType.Expense, TransactionCategory? category = TransactionCategory.Debit, decimal? value = null, DateTime? date = null, string? description = null)
    {
        _id = id ?? 1;
        _accountId = accountId ?? 1;
        _type = type ?? TransactionType.Expense;
        _category = category ?? TransactionCategory.Electricity;
        _value = value ?? 100.00M;
        _description = description ?? "Default Transaction";
        _date = date ?? DateTime.Now; ;
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
