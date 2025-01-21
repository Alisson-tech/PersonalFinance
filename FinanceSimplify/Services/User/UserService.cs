using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;

namespace FinanceSimplify.Services.Transaction;

public class UserService : IUserService
{
    private readonly IGenericRepository<Users> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IGenericRepository<Users> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Task<string> CreateUser(UserCreate user)
    {
        throw new NotImplementedException();
    }

    public Task<TokenDto> Login(UserLogin user)
    {
        throw new NotImplementedException();
    }

    public Task<TokenDto> Refresh(string tokenRefresh)
    {
        throw new NotImplementedException();
    }
}
