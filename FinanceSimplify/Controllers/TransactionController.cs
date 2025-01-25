using FinanceSimplify.Exceptions;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Services.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService TransactionService)
    {
        _transactionService = TransactionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetAll([FromQuery] TransactionFilter filter, [FromQuery] PaginatedFilter page)
    {
        try
        {
            return Ok(await _transactionService.GetTransactionList(filter, page));
        }
        catch
        {
            return BadRequest("Erro interno");
        }

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetId(int id)
    {
        try
        {
            return Ok(await _transactionService.GetTransaction(id));
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
    public async Task<ActionResult<TransactionDto>> Create([FromBody] TransactionCreate TransactionDto)
    {
        try
        {
            return Ok(await _transactionService.CreateTransaction(TransactionDto));
        }
        catch (FinanceInternalErrorException ex)
        {
            return BadRequest(ex.Message);
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _transactionService.DeleteTransaction(id);
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