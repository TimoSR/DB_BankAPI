using API.Features.CreateAccount.Application;
using API.Features.CreateAccount.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSecurityDomainService _security;

    public AccountController(
        IAccountRepository accountRepository,
        IAccountSecurityDomainService security)
    {
        _accountRepository = accountRepository;
        _security = security;
    }

    [HttpGet("get-all-accounts")]
    public ActionResult<List<AccountDto>> GetAllAccounts()
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

    [HttpGet("get-account-by-id")]
    public ActionResult<AccountDto> GetAccountById(string id)
    {
        var account = _accountRepository.GetAccount(id);
        
        var accountDto = new AccountDto()
        {
            Id = account.Id,
            CPR = account.CPR,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Balance = account.Balance
        };

        return accountDto;
    }

    [HttpGet("get-balance-by-id")]
    public ActionResult<decimal> GetBalanceById(string id)
    {
        var balance = _accountRepository.GetBalance(id);

        return balance;
    }

    [HttpPost("create-account")]
    public IActionResult CreateAccount(CreateAccountRequest request)
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