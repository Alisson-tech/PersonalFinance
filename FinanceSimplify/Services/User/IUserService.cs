namespace FinanceSimplify.Services.Transaction;

public interface IUserService
{
    public Task<string> CreateUser(UserCreate user);
    public Task<TokenDto> Login(UserLogin user);
    public Task<TokenDto> Refresh(string tokenRefresh);
}
