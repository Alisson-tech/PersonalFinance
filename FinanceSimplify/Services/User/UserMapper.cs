using AutoMapper;
using FinanceSimplify.Data;

namespace FinanceSimplify.Services.Transaction;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserCreate, Users>();
    }
}

