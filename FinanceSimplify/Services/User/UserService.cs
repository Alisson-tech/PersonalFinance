using AutoMapper;
using FinanceSimplify.Data;
using FinanceSimplify.Exceptions;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinanceSimplify.Services.Transaction;

public class UserService : IUserService
{
    private readonly IGenericRepository<Users> _userRepository;
    private readonly IMapper _mapper;
    private readonly TokenGenerate _tokenGenerate;

    public UserService(IGenericRepository<Users> userRepository, IMapper mapper, TokenGenerate tokenGenerate)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _tokenGenerate = tokenGenerate;
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

        return CreateToken(user.Id.ToString(), user.Name, user.Email);
    }

    public Task<TokenDto> Refresh(string tokenRefresh)
    {
        throw new NotImplementedException();
    }

    private TokenDto CreateToken(string userId, string name, string email)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email)
        };

        return new TokenDto()
        {
            Token = _tokenGenerate.GenerateAccessToken(claims),
            RefreshToken = _tokenGenerate.GenerateRefreshToken(userId)
        };
    }
}
