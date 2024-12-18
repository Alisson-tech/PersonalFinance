using AutoMapper;
using AutoMapper.QueryableExtensions;
using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Infrastructure;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Account;

namespace FinanceSimplify.Services.Transaction;

public class TransactionService : ITransactionService
{
    IGenericRepository<Transactions> _transactionRepository;
    IMapper _mapper;

    public TransactionService(IGenericRepository<Transactions> TransactionRepository, IMapper mapper)
    {
        _transactionRepository = TransactionRepository;
        _mapper = mapper;
    }

    public async Task<TransactionDto> CreateTransaction(TransactionCreate TransactionCreate)
    {
        ValidateTransactionCreate(TransactionCreate);

        var Transaction = _mapper.Map<Transactions>(TransactionCreate);

        var createdTransaction = await _transactionRepository.Create(Transaction);

        return _mapper.Map<TransactionDto>(createdTransaction);
    }

    public async Task<TransactionDto> UpdateTransaction(int id, TransactionCreate TransactionCreate)
    {
        ValidateTransactionCreate(TransactionCreate);

        var Transaction = _mapper.Map<Transactions>(TransactionCreate);

        var updatedTransaction = await _transactionRepository.Update(id, Transaction);

        return _mapper.Map<TransactionDto>(updatedTransaction);
    }

    public async Task<TransactionDto> GetTransaction(int id)
    {
        var Transaction = await _transactionRepository.GetById(id);

        return _mapper.Map<TransactionDto>(Transaction);
    }

    public async Task<PaginatedList<TransactionDto>> GetTransactionList(TransactionFilter filter, PaginatedFilter pageFilter)
    {
        var Transactions = _transactionRepository.GetList();

        if (filter.Type != null)
        {
            Transactions = Transactions.Where(x => x.Type == filter.Type);
        }

        return await Transactions
            .OrderByDynamic(pageFilter.OrderBy, pageFilter.OrderAsc)
            .ProjectTo<TransactionDto>(_mapper.ConfigurationProvider)
            .ToPagedResultAsync(pageFilter.PageNumber, pageFilter.PageSize);
    }

    public async Task DeleteTransaction(int id)
    {
        await _transactionRepository.HardDelete(id);
    }

    private static void ValidateTransactionCreate(TransactionCreate TransactionCreate)
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
}
