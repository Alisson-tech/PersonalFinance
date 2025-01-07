using FinanceSimplify.Infraestructure;

namespace FinanceSimplify.Services.Transaction;

public interface ITransactionService
{
    public Task<PaginatedList<TransactionDto>> GetTransactionList(TransactionFilter filter, PaginatedFilter pageFilter);
    public Task<TransactionDto> GetTransaction(int id);
    public Task<TransactionDto> CreateTransaction(TransactionCreate transactionCreate);
    public Task DeleteTransaction(int id);
}
