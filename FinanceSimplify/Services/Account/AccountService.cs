using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceSimplify.Data;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Infrastructure;
using FinanceSimplify.Repositories;


namespace FinanceSimplify.Services.Account;

public class AccountService : IAccountService
{
    IGenericRepository<Accounts> _accountRepository;
    IMapper _mapper;

    public AccountService(IGenericRepository<Accounts> accountRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<AccountDto> CreateAccount(AccountCreate accountCreate)
    {
        var account = _mapper.Map<Accounts>(accountCreate);

        account.Validate();

        var createdAccount = await _accountRepository.Create(account);

        return _mapper.Map<AccountDto>(createdAccount);
    }

    public async Task<AccountDto> UpdateAccount(int id, AccountCreate accountCreate)
    {
        var account = _mapper.Map<Accounts>(accountCreate);

        account.Validate();

        var updatedAccount = await _accountRepository.Update(id, account);

        return _mapper.Map<AccountDto>(updatedAccount);
    }

    public async Task<AccountDto> GetAccount(int id)
    {
        var account = await _accountRepository.GetById(id);

        return _mapper.Map<AccountDto>(account);
    }

    public async Task<PaginatedList<AccountDto>> GetAccountList(AccountsFilter filter, PaginatedFilter pageFilter)
    {
        var accounts = _accountRepository.GetIqueryble();

        if (filter.Type != null)
        {
            accounts = accounts.Where(x => x.Type == filter.Type);
        }

        return await accounts
            .OrderByDynamic(pageFilter.OrderBy, pageFilter.OrderAsc)
            .ProjectTo<AccountDto>(_mapper.ConfigurationProvider)
            .ToPagedResultAsync(pageFilter.PageNumber, pageFilter.PageSize);
    }

    public async Task DeleteAccount(int id)
    {
        await _accountRepository.SoftDelete(id);
    }
}
