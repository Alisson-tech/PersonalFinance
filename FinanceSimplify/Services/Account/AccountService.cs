using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Shared;
using Microsoft.EntityFrameworkCore;


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

    public async Task<AccountDto> CreateAccount(AccountDto accountDto)
    {
        var account = _mapper.Map<Accounts>(accountDto);

        var createdAccount = await _accountRepository.Create(account);

        return _mapper.Map<AccountDto>(createdAccount);
    }

    public async Task<AccountDto> UpdateAccount(int id, AccountDto accountDto)
    {
        var account = _mapper.Map<Accounts>(accountDto);

        var updatedAccount = await _accountRepository.Update(id, account);

        return _mapper.Map<AccountDto>(updatedAccount);
    }

    public async Task<AccountDto> GetAccount(int id)
    {
        var account = await _accountRepository.GetById(id);

        return _mapper.Map<AccountDto>(account);
    }

    public async Task<List<AccountDto>> GetAccountList(AccountsFilter filter, PaginatedFilter pageFilter)
    {
        var accounts = _accountRepository.GetAll();

        if (filter.Type != null)
        {
            accounts = accounts.Where(x => x.Type == filter.Type);
        }

        return await accounts.ProjectTo<AccountDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task DeleteAccount(int id)
    {
        await _accountRepository.SoftDelete(id);
    }
}
