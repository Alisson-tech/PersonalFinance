using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Repositories;
using Microsoft.EntityFrameworkCore;

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

    public async Task<string> CreateUser(UserCreate userCreate)
    {
        var user = _mapper.Map<Users>(userCreate);

        user.Validate();

        var userResult = await _userRepository.Create(user);

        return userResult.Name;
    }

    public async Task<TokenDto> Login(UserLogin userLogin)
    {
        var user = await _userRepository.GetIqueryble()
            .Where(u => u.Email == userLogin.Email && u.Password == userLogin.Password).FirstOrDefaultAsync();

        if (user == null)
            throw new FinanceUnauthorizedException("Login inválido");

        return new TokenDto()
        {
            Token = "teste",
            RefreshToken = "teste"
        };
    }

    public Task<TokenDto> Refresh(string tokenRefresh)
    {
        throw new NotImplementedException();
    }
}
