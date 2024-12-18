using FinanceSimplify.Infraestructure;

namespace FinanceSimplify.Services.Account;

public interface IAccountService
{
    public Task<PaginatedList<AccountDto>> GetAccountList(AccountsFilter filter, PaginatedFilter pageFilter);
    public Task<AccountDto> GetAccount(int id);
    public Task<AccountDto> CreateAccount(AccountCreate accountCreate);
    public Task<AccountDto> UpdateAccount(int id, AccountCreate accountCreate);
    public Task DeleteAccount(int id);
}
