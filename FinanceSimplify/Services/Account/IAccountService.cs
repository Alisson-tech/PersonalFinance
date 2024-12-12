using FinanceSimplify.Services.Shared;

namespace FinanceSimplify.Services.Account;

public interface IAccountService
{
    public Task<List<AccountDto>> GetAccountList(AccountsFilter filter, PaginatedFilter pageFilter);
    public Task<AccountDto> GetAccount(int id);
    public Task<AccountDto> CreateAccount(AccountDto accountDto);
    public Task<AccountDto> UpdateAccount(int id, AccountDto accountDto);
    public Task DeleteAccount(int id);
}
