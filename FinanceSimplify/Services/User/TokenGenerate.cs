using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceSimplify.Services.User;

public class TokenGenerate
{
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _cache;

    public TokenGenerate(IConfiguration configuration, IMemoryCache cache)
    {
        _configuration = configuration;
        _cache = cache;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(20),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(string userEmail)
    {
        var refreshToken = Guid.NewGuid().ToString();
        var cacheKey = $"refreshToken_{userEmail}_{refreshToken}";

        _cache.Set(cacheKey, refreshToken, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });

        return refreshToken;
    }

    public bool ValidateRefreshToken(string userEmail, string refreshToken)
    {
        var cacheKey = $"refreshToken_{userEmail}_{refreshToken}";
        var cachedValue = _cache.Get(cacheKey)?.ToString();

        if (cachedValue == refreshToken)
        {
            _cache.Remove(cacheKey);
            return true;
        }

        return false;
    }
}
