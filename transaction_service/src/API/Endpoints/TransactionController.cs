using API.Features.Application;
using API.Features.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class TransactionController(ITransactionService service) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<Transaction>>> GetAll()
    {
        var result = await service.GetAllTransactionsAsync();
        return Ok(result.Data);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<Transaction>> GetById(string id)
    {
        var result = await service.GetTransactionByIdAsync(id);
        return Ok(result.Data);
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateTransactionCommand command)
    {
        var result = await service.CreateTransactionAsync(command);
        return Ok(result.Messages);
    }
}