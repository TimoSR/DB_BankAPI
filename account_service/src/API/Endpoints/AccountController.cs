using API.Features.Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<AccountDTO>>> GetAll()
    {
        var result = await service.GetAllAccountsAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        
        return BadRequest(result.Messages);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<AccountDTO>> GetById(string id)
    {
        var result = await service.GetAccountByIdAsync(id);
        
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        
        return BadRequest(result.Messages);
    }

    [HttpGet("GetBalanceById")]
    public async Task<ActionResult<decimal>> GetBalanceById(string id)
    {
        var result = await service.GetBalanceByIdAsync(id);
        
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        
        return BadRequest(result.Messages);
    }

    [HttpPost("CreateAccount")]
    public async Task<IActionResult> CreateAccount(CreateAccountCommand command)
    {
        var result = await service.CreateAccountAsync(command);
        
        if (result.IsSuccess)
        {
            return Ok(result.Messages);
        }
        
        return BadRequest(result.Messages);
    }
}