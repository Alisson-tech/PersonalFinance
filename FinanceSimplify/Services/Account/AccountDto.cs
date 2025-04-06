using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Account;

public class AccountDto
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class AccountCreate
{
    public AccountType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class AccountsFilter
{
    public AccountType? Type { get; set; }
}