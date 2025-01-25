﻿using FinanceSimplify.Services.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FinanceSimplify.Test.UnitTest;

public class TokenTest
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly MemoryCache _mockCache;
    private readonly TokenGenerate _tokenGenerate;

    public TokenTest()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockCache = new MemoryCache(new MemoryCacheOptions());

        // Mockando as configurações Jwt
        _mockConfiguration.Setup(config => config.GetSection("Jwt")["Key"]).Returns("supersecretkey#@88888+testUnitKey");
        _mockConfiguration.Setup(config => config.GetSection("Jwt")["Issuer"]).Returns("testIssuer");
        _mockConfiguration.Setup(config => config.GetSection("Jwt")["Audience"]).Returns("testAudience");

        _tokenGenerate = new TokenGenerate(_mockConfiguration.Object, _mockCache);
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnToken()
    {
        // Arrange
        var name = "testUser";
        var email = "testUser@gmail.com";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email)
        };

        // Act
        var token = _tokenGenerate.GenerateAccessToken(claims);
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(token);

        // Assert
        Assert.NotNull(token);
        Assert.IsType<string>(token);
        Assert.Contains("eyJ", token);
        Assert.Contains(decodedToken.Claims, c => c.Value == name);
        Assert.Contains(decodedToken.Claims, c => c.Value == email);
    }

    [Fact]
    public void GenerateRefreshToken_ShouldReturnRefreshToken()
    {
        // Arrange
        var userEmail = "test@gmail.com";

        // Act
        var refreshToken = _tokenGenerate.GenerateRefreshToken(userEmail);

        // Assert
        Assert.NotNull(refreshToken);
        Assert.IsType<string>(refreshToken);
        Assert.IsType<Guid>(Guid.Parse(refreshToken));
    }

    [Fact]
    public void ValidateValidRefreshToken_ShouldReturnTrue()
    {
        // Arrange
        var userEmail = "test@gmail.com";
        var refreshToken = Guid.NewGuid().ToString();
        var cacheKey = $"refreshToken_{userEmail}_{refreshToken}";

        _mockCache.Set(cacheKey, refreshToken);

        // Act
        var result = _tokenGenerate.ValidateRefreshToken(userEmail, refreshToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateInvalidRefreshToken_ShouldReturnFalse()
    {
        // Arrange
        var userEmail = "test@gmail.com";
        var refreshToken = "invalidRefreshToken";
        var cacheKey = $"refreshToken:{userEmail}:{refreshToken}";

        // Act
        var result = _tokenGenerate.ValidateRefreshToken(userEmail, refreshToken);

        // Assert
        Assert.False(result);
    }
}
