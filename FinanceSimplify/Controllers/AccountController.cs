using FinanceSimplify.Exceptions;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Services.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<AccountDto>>> GetAll([FromQuery] AccountsFilter filter, [FromQuery] PaginatedFilter page)
    {
        return Ok(await _accountService.GetAccountList(filter, page));

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetId(int id)
    {
        try
        {
            return Ok(await _accountService.GetAccount(id));
        }
        catch (FinanceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> Create([FromBody] AccountCreate accountDto)
    {
        try
        {
            return Ok(await _accountService.CreateAccount(accountDto));
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

    [HttpPut("{id}")]
    public async Task<ActionResult<AccountDto>> Update(int id, [FromBody] AccountCreate accountDto)
    {
        try
        {
            return Ok(await _accountService.UpdateAccount(id, accountDto));
        }
        catch (FinanceNotFoundException ex)
        {
            return NotFound(ex.Message);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _accountService.DeleteAccount(id);
            return Ok("Conta deletada com sucesso");
        }
        catch (FinanceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return BadRequest("Erro interno");
        }
    }

}
