using AutoMapper;
using FinanceSimplify.Context;
using FinanceSimplify.Controllers;
using FinanceSimplify.Data;
using FinanceSimplify.Repositories;
using FinanceSimplify.Services.Transaction;
using FinanceSimplify.Test.IntegrationTesting.Context;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Test.IntegrationTest;

public class UserControllerTest
{
    private readonly ContextFinanceTest _contextTest;

    public UserControllerTest()
    {
        _contextTest = new ContextFinanceTest();
    }


    // teste create user
    [Fact]
    public async Task CreateValidUser_ShouldReturnNameUser()
    {
        // arrange
        var context = _contextTest.CreateContext();
        var userController = GetUserController(context);
        var nameAccount = "new account";

        var userCreate = new UserCreate()
        {
            Email = "new@gmail.com",
            Name = nameAccount,
            Password = "password123",
        };

        //act
        var result = await userController.Create(userCreate);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult);

        var response = Assert.IsAssignableFrom<string>(okResult.Value);

        Assert.NotNull(response);
        Assert.Equal(response, nameAccount);
    }

    [Theory]
    [InlineData("", "validEmail@example.com", "ValidPassword123")]
    [InlineData("Valid Name", "validemail@example.com", "")]
    [InlineData("Valid Name", "validemail@example.com", "1234567")]
    public async Task CreateInvalidUser_ShouldReturnBadRequest(string name, string email, string password)
    {
        // arrange
        var context = _contextTest.CreateContext();
        var userController = GetUserController(context);

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


    //teste create invalid user

    // teste login

    // teste invalid login

    private static UserController GetUserController(ContextFinance context)
    {
        var userRepository = new GenericRepository<Users>(context);
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<UserMapper>()));
        var service = new UserService(userRepository, mapper);

        return new UserController(service);
    }
}
