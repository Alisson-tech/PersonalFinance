using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Controllers;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Transaction;
using FinanceSimplify.Services.User;
using FinanceSimplify.Test.IntegrationTest.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinanceSimplify.Test.IntegrationTest;

public class UserControllerTest
{
    private readonly ContextFinanceTest _contextTest;

    public UserControllerTest()
    {
        _contextTest = new ContextFinanceTest();
    }

    [Fact]
    public async Task CreateValidUser_ShouldReturnNameUser()
    {
        // arrange
        var context = _contextTest.CreateContext();
        var cache = GetMemoryCache();
        var userController = GetUserController(context, cache);
        var UserName = "new account";

        var userCreate = new UserCreate()
        {
            Email = "new@gmail.com",
            Name = UserName,
            Password = "password123",
        };

        //act
        var result = await userController.Create(userCreate);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<string>(okResult.Value);

        Assert.NotNull(response);
        Assert.Equal(response, UserName);
    }

    [Theory]
    [InlineData("", "validEmail@example.com", "ValidPassword123")]
    [InlineData("Valid Name", "validemail@example.com", "")]
    [InlineData("Valid Name", "validemail@example.com", "1234567")]
    public async Task CreateInvalidUser_ShouldReturnBadRequest(string name, string email, string password)
    {
        // arrange
        var context = _contextTest.CreateContext();
        var cache = GetMemoryCache();
        var userController = GetUserController(context, cache);

        var userCreate = new UserCreate()
        {
            Email = name,
            Name = email,
            Password = password,
        };

        //Act & assert
        var result = await userController.Create(userCreate);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task LoginUser_ShouldReturnToken()
    {
        // arrange
        var context = _contextTest.CreateContext();
        var cache = GetMemoryCache();
        var userController = GetUserController(context, cache);
        var email = "new@gmail.com";
        var password = "password123";

        var userCreate = new Users()
        {
            Email = email,
            Name = "teste",
            Password = password,
        };

        await context.AddAsync(userCreate);
        await context.SaveChangesAsync();

        //act
        var userLogin = new UserLogin
        {
            Email = email,
            Password = password
        };
        var result = await userController.Login(userLogin);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<TokenDto>(okResult.Value);
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(response.Token);

        Assert.NotNull(response);
        Assert.Contains(decodedToken.Claims, c => c.Value == email);
        Assert.IsType<Guid>(Guid.Parse(response.RefreshToken));
    }

    [Theory]
    [InlineData("new@gmail.com", "")]
    [InlineData("", "password123")]
    public async Task LoginInvalidUser_ShouldReturnUnauthorized(string email, string password)
    {
        // arrange
        var context = _contextTest.CreateContext();
        var cache = GetMemoryCache();
        var userController = GetUserController(context, cache);

        var userCreate = new Users()
        {
            Email = "new@gmail.com",
            Name = "teste",
            Password = "password123",
        };

        await context.AddAsync(userCreate);

        //act
        var userLogin = new UserLogin
        {
            Email = email,
            Password = password
        };
        var result = await userController.Login(userLogin);

        //assert
        Assert.IsType<UnauthorizedObjectResult>(result.Result);
    }

    [Fact]
    public void RefreshTokenValid_ShouldReturnTokenDto()
    {
        // arrange
        var email = "teste@gmail.com";
        var name = "teste";
        var httpContext = GetHttpContext(name, email);

        var context = _contextTest.CreateContext();
        var cache = GetMemoryCache();
        var userController = GetUserController(context, cache);
        userController.ControllerContext = new ControllerContext() { HttpContext = httpContext };

        string refreshToken = Guid.NewGuid().ToString();
        cache.Set($"refreshToken_{email}_{refreshToken}", refreshToken, TimeSpan.FromMinutes(5));

        // Act
        var result = userController.Refresh(refreshToken);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<TokenDto>(okResult.Value);

        Assert.NotNull(response);
        Assert.IsType<Guid>(Guid.Parse(response.RefreshToken));
        Assert.NotEqual(refreshToken, response.RefreshToken);

    }

    [Fact]
    public void RefreshTokenInvalid_ShouldReturnUnauthorized()
    {
        // arrange
        var email = "teste@gmail.com";
        var name = "teste";
        var httpContext = GetHttpContext(email, name);

        var context = _contextTest.CreateContext();
        var cache = GetMemoryCache();
        var userController = GetUserController(context, cache);

        userController.ControllerContext = new ControllerContext() { HttpContext = httpContext };

        string refreshToken = Guid.NewGuid().ToString();

        // Act
        var result = userController.Refresh(refreshToken);

        //assert
        Assert.IsType<UnauthorizedObjectResult>(result.Result);

    }

    private static UserController GetUserController(ContextFinance context, MemoryCache memoryCache)
    {
        var cache = memoryCache;
        var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

        var userRepository = new GenericRepository<Users>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>()));
        var tokenGenerate = new TokenGenerate(configuration, cache);

        var service = new UserService(userRepository, mapper, tokenGenerate);

        return new UserController(service);
    }

    private static MemoryCache GetMemoryCache()
    {
        return new MemoryCache(new MemoryCacheOptions());
    }

    private static HttpContext GetHttpContext(string username, string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Email, email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tested54a6aa74da7ad+#$dalsdla=dlsda"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.WriteToken(token);

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(r => r.Headers["Authorization"]).Returns($"Bearer {accessToken}");

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(context => context.Request).Returns(mockRequest.Object);


        return mockHttpContext.Object;
    }
}
