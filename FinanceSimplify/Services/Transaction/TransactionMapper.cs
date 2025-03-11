using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Infrastructure;

namespace FinanceSimplify.Services.Transaction;

public class TransactionMapper : Profile
{
    public TransactionMapper()
    {
        CreateMap<Transactions, TransactionDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.GetDisplayName()))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.GetDisplayName()));
        CreateMap<TransactionCreate, Transactions>()
            .ForMember(dest => dest.Account, opt => opt.Ignore());
    }
}

