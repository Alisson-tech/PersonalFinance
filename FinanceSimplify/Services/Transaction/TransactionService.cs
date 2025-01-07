using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Infrastructure;
using FinanceSimplify.Repositories;

namespace FinanceSimplify.Services.Transaction;

public class TransactionService : ITransactionService
{
    private readonly IGenericRepository<Transactions> _transactionRepository;
    private readonly IGenericRepository<Accounts> _accountRepository;
    private readonly IMapper _mapper;

    public TransactionService(IGenericRepository<Transactions> transactionRepository, IGenericRepository<Accounts> accountRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDto> CreateTransaction(TransactionCreate transactionCreate)
    {
        ValidateTransactionTypes(transactionCreate);

        var account = await GetAccountById(transactionCreate.AccountId);
        CalculatorValueAccount(account, transactionCreate.Type, transactionCreate.Value);

        var Transaction = _mapper.Map<Transactions>(transactionCreate);

        var createdTransaction = await _transactionRepository.Create(Transaction);

        return _mapper.Map<TransactionDto>(createdTransaction);
    }

    public async Task<TransactionDto> GetTransaction(int id)
    {
        var Transaction = await _transactionRepository.GetById(id);

        return _mapper.Map<TransactionDto>(Transaction);
    }

    public async Task<PaginatedList<TransactionDto>> GetTransactionList(TransactionFilter filter, PaginatedFilter pageFilter)
    {
        var Transactions = _transactionRepository.GetIqueryble()
            .Where(t => (filter.AccountId == null || t.AccountId == filter.AccountId) &&
                (filter.Category == null || t.Category == filter.Category) &&
                (filter.Description == null || t.Description == filter.Description) &&
                (filter.DateStart == null || t.Date >= filter.DateStart) &&
                (filter.DateFinish == null || t.Date <= filter.DateFinish));

        return await Transactions
            .OrderByDynamic(pageFilter.OrderBy, pageFilter.OrderAsc)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToPagedResultAsync(pageFilter.PageNumber, pageFilter.PageSize);
    }

    public async Task DeleteTransaction(int id)
    {
        var transaction = await _transactionRepository.GetById(id);
        var account = await GetAccountById(transaction.AccountId);
        CalculatorValueAccount(account, transaction.Type, transaction.Value);

        await _transactionRepository.HardDelete(id);
    }

    private async Task<Accounts> GetAccountById(int accountId)
    {
        var account = await _accountRepository.GetById(accountId);

        if (account == null)
        {
            throw new FinanceNotFoundException("Conta não encontrada");
        }

        return account;
    }

    private static void ValidateTransactionTypes(TransactionCreate TransactionCreate)
    {
        if (!Enum.IsDefined(typeof(TransactionCategory), TransactionCreate.Category))
        {
            throw new FinanceInternalErrorException($"categoria Inválida");
        }

        if (!Enum.IsDefined(typeof(TransactionType), TransactionCreate.Type))
        {
            throw new FinanceInternalErrorException("tipo Inválido");
        }
    }

    private static void CalculatorValueAccount(Accounts account, TransactionType transactionType, decimal transactionValue)
    {
        if (transactionType == TransactionType.Expense)
        {
            account.Balance -= transactionValue;
        }
        else
        {
            account.Balance += transactionValue;
        }
    }
}
