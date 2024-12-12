using AutoMapper;
using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Account;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<Accounts, AccountDto>().ReverseMap();

    }
}
