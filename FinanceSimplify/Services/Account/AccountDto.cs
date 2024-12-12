using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Account;

public class AccountDto
{
    public AccountType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}

public class AccountsFilter
{
    public AccountType? Type { get; set; }
}