using API.Features.CreateAccount.Application;
using API.Features.CreateAccount.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSecurityDomainService _security;

    public AccountsController(
        IAccountRepository accountRepository,
        IAccountSecurityDomainService security
        )
    {
        _accountRepository = accountRepository;
        _security = security;
    }

    [HttpGet]
    public ActionResult<List<AccountDto>> Get()
    {
        
        var accounts = _accountRepository.GetAccounts();
        var accountDTOs = new List<AccountDto>(); 
            
        foreach (var account in accounts)
        {
            accountDTOs.Add(new AccountDto()
            {
                Id = account.Id,
                CPR = account.CPR,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Balance = account.Balance
            });
        }

        return accountDTOs;
    }

    [HttpPost]
    public IActionResult Create(CreateAccountRequest request)
    {
        var account = new Account()
        {
            CPR = _security.Hash(request.CPR),
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        _accountRepository.CreateAccount(account);
        return Ok();
    }
}