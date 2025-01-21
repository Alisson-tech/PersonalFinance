using FinanceSimplify.Exceptions;
using FinanceSimplify.Services.Transaction;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
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
}
