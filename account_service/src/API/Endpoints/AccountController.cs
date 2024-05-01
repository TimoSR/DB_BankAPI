using API.Features.Application;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpGet("get-all-accounts")]
    public async Task<ActionResult<List<AccountDTO>>> GetAllAccounts()
    {
        var result = await service.GetAllAccountsAsync();
        return Ok(result.Data);
    }

    [HttpGet("get-account-by-id")]
    public async Task<ActionResult<AccountDTO>> GetAccountById(string id)
    {
        var result = await service.GetAccountByIdAsync(id);
        return Ok(result.Data);
    }

    [HttpGet("get-balance-by-id")]
    public async Task<ActionResult<decimal>> GetBalanceById(string id)
    {
        var result = await service.GetBalanceByIdAsync(id);
        return Ok(result.Data);
    }

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount(CreateAccountRequest request)
    {
        var result = await service.CreateAccountAsync(request);
        return Ok(result.Messages);
    }
}