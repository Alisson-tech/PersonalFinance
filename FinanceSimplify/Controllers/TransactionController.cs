using FinanceSimplify.Exceptions;
using FinanceSimplify.Infraestructure;
using FinanceSimplify.Services.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace FinanceSimplify.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : Controller
{
    private readonly ITransactionService _TransactionService;

    public TransactionController(ITransactionService TransactionService)
    {
        _TransactionService = TransactionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionDto>>> GetAll([FromQuery] TransactionFilter filter, [FromQuery] PaginatedFilter page)
    {
        try
        {
            return Ok(await _TransactionService.GetTransactionList(filter, page));
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
            return Ok(await _TransactionService.GetTransaction(id));
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
            return Ok(await _TransactionService.CreateTransaction(TransactionDto));
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
    public async Task<ActionResult<TransactionDto>> Update(int id, [FromBody] TransactionCreate TransactionDto)
    {
        try
        {
            return Ok(await _TransactionService.UpdateTransaction(id, TransactionDto));
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
            await _TransactionService.DeleteTransaction(id);
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