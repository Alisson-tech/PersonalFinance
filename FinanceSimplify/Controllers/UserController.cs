using FinanceSimplify.Exceptions;
using FinanceSimplify.Services.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("create")]
    public async Task<ActionResult<string>> Create([FromBody] UserCreate user)
    {
        try
        {
            return Ok(await _userService.CreateUser(user));
        }
        catch (FinanceInternalErrorException ex)
        {
            return BadRequest(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }

    }

    [HttpPost]
    public async Task<ActionResult<TokenDto>> Login([FromBody] UserLogin userLongin)
    {
        try
        {
            return Ok(await _userService.Login(userLongin));
        }
        catch (FinanceUnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }
    }

    [HttpPost("/refresh")]
    public ActionResult<TokenDto> Refresh([FromBody] string refreshToken)
    {
        try
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return Ok(_userService.RefreshToken(name, email, refreshToken));
        }
        catch (FinanceUnauthorizedException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }
    }
}
