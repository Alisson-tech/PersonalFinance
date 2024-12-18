using AutoMapper;
using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Transaction;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<Transactions, TransactionDto>();
        CreateMap<TransactionCreate, Transactions>();
            
    }
}

