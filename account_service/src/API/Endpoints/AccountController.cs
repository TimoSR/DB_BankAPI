using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    public AccountController() {}

    [HttpGet("get-hello-world")]
    public IActionResult GetHelloWorld()
    {
        var hello = "Hello World";
        
        return Ok(hello);
    }
}