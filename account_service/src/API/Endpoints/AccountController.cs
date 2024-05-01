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
        return service.GetAllAccounts();
    }

    [HttpGet("get-account-by-id")]
    public ActionResult<AccountDto> GetAccountById(string id)
    {
        return service.GetAccountById(id);
    }

    [HttpGet("get-balance-by-id")]
    public ActionResult<decimal> GetBalanceById(string id)
    {
        return service.GetBalanceById(id);
    }

    [HttpPost("create-account")]
    public IActionResult CreateAccount(CreateAccountRequest request)
    {
        service.CreateAccount(request);

        return Ok();
    }
}