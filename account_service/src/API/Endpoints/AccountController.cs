using API.Features.Application;
using API.Features.CreateAccount.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService service) : ControllerBase
{
    [HttpGet("get-all-accounts")]
    public ActionResult<List<AccountDto>> GetAllAccounts()
    {
        var result = service.GetAllAccounts();
        return Ok(result.Data);
    }

    [HttpGet("get-account-by-id")]
    public ActionResult<AccountDto> GetAccountById(string id)
    {
        var result = service.GetAccountById(id);
        return Ok(result.Data);
    }

    [HttpGet("get-balance-by-id")]
    public ActionResult<decimal> GetBalanceById(string id)
    {
        var result = service.GetBalanceById(id);
        return Ok(result.Data);
    }

    [HttpPost("create-account")]
    public IActionResult CreateAccount(CreateAccountRequest request)
    {
        var result = service.CreateAccount(request);

        return Ok(result.Messages);
    }
}