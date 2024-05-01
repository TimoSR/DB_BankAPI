using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    public AccountController() {}
    
    [HttpPut("create-account")]
    public IActionResult CreateAccount()
    {
        throw new NotImplementedException();
    }
}