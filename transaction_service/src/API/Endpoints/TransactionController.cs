using API.Features.Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("api/[controller]")]
public class TransactionController(ITransactionService service) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<TransactionDTO>>> GetAll()
    {
        var result = await service.GetAllTransactionsAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.Messages);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<TransactionDTO>> GetById(string id)
    {
        var result = await service.GetTransactionByIdAsync(id);
        
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.Messages);
    }

    [HttpGet("GetLast10AccountTransaction")]
    public async Task<ActionResult<List<TransactionDTO>>> GetLast10AccountTransaction(string id)
    {
        var result = await service.GetLast10AccountTransactionsAsync(id);
        
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest(result.Messages);
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateTransactionCommand command)
    {
        var result = await service.CreateTransactionAsync(command);

        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }
        
        return BadRequest(result.Messages);
    }
}