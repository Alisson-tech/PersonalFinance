using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Infrastructure;

namespace FinanceSimplify.Services.Account;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<Accounts, AccountDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.GetDisplayName()));
        CreateMap<AccountCreate, Accounts>();
    }
}
