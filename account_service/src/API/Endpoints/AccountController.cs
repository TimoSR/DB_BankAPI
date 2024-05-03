using API.Features.Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpGet("GetAll")]
    public async Task<ActionResult<List<AccountDTO>>> GetAll()
    {
        var result = await service.GetAllAccountsAsync();
        return Ok(result.Data);
    }

    [HttpGet("GetById")]
    public async Task<ActionResult<AccountDTO>> GetById(string id)
    {
        var result = await service.GetAccountByIdAsync(id);
        return Ok(result.Data);
    }

    [HttpGet("GetBalanceById")]
    public async Task<ActionResult<decimal>> GetBalanceById(string id)
    {
        var result = await service.GetBalanceByIdAsync(id);
        return Ok(result.Data);
    }

    [HttpPost("CreateAccount")]
    public async Task<IActionResult> CreateAccount(CreateAccountCommand command)
    {
        var result = await service.CreateAccountAsync(command);
        return Ok(result.Messages);
    }
    
    // Test Endpoint
    /*[HttpPut("UpdateBalance")]
    public async Task<IActionResult> UpdateBalance(UpdateBalanceRequest request)
    {
        var result = await service.UpdateAccountBalanceAsync(request);
        return Ok(result.Messages);
    }*/
}