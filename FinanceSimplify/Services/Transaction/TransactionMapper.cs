using AutoMapper;
using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Transaction;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<Transactions, TransactionDto>()
            .ForMember(dest => dest.AccountName, map => map.MapFrom(src => src.Account.Name));
        CreateMap<TransactionCreate, Transactions>();
            
    }
}

