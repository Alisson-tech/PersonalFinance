using FinanceSimplify.Exceptions;
using System.Text.RegularExpressions;

namespace FinanceSimplify.Data;

public class Users
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void Validate()
    {
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (string.IsNullOrEmpty(Name))
            throw new FinanceInternalErrorException("Nome inválido");
        if (string.IsNullOrEmpty(Email) || !Regex.IsMatch(Email, emailRegex))
            throw new FinanceInternalErrorException("Email inválido");
        if (string.IsNullOrEmpty(Password) || Password.Length < 8)
            throw new FinanceInternalErrorException("Password inválido");
    }
}
