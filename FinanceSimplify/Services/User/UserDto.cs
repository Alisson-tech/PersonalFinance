namespace FinanceSimplify.Services.Transaction;


public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public class UserCreate
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserLogin
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

