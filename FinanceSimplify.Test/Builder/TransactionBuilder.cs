using FinanceSimplify.Data;

namespace FinanceSimplify.Test.Builder;

public class TransactionBuilder
{
    private int _id = 1;
    private int _accountId = 1;
    private TransactionType _type = TransactionType.Expense;
    private TransactionCategory _category = TransactionCategory.Debit;
    private decimal _value = 100m;
    private DateTime _date = DateTime.Now;
    private string _description = "Default Transaction";

    public TransactionBuilder CreateDefault(int? id = null, int? accountId = null, TransactionType? type = TransactionType.Expense, TransactionCategory? category = TransactionCategory.Debit, decimal? value = null, DateTime? date = null, string? description = null)
    {
        _id = id ?? _id;
        _accountId = accountId ?? 1;
        _type = type ?? TransactionType.Expense;
        _category = category ?? TransactionCategory.Debit;
        _value = value ?? 100.00M;
        _description = description ?? "Default Transaction";
        _date = date ?? DateTime.Now;

        return this;
    }

    public List<Transactions> BuildRandom(int quantity)
    {
        var random = new Random();
        var transactionsList = new List<Transactions>();

        for (int i = 0; i < quantity; i++)
        {
            transactionsList.Add(new Transactions
            {
                Id = _id + i,
                AccountId = (int)random.Next(1, 2),
                Type = (TransactionType)random.Next(0, Enum.GetValues(typeof(TransactionType)).Length),
                Category = (TransactionCategory)random.Next(0, Enum.GetValues(typeof(TransactionCategory)).Length),
                Value = Math.Round((decimal)random.NextDouble() * 1000, 2),
                Description = $"Random Transaction {i + 1}",
                Date = DateTime.Now,
            });
        }

        return transactionsList;
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
