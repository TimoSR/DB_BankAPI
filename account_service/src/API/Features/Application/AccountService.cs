using API.Features.CreateAccount.Domain;

namespace API.Features.Application;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSecurityDomainService _security;

    public AccountService( 
        IAccountRepository accountRepository,
        IAccountSecurityDomainService security)
    {
        _accountRepository = accountRepository;
        _security = security;
    }
    
    public List<AccountDto> GetAllAccounts()
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

    public AccountDto GetAccountById(string id)
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

    public void CreateAccount(CreateAccountRequest request)
    {
        var account = new Account()
        {
            CPR = _security.Hash(request.CPR),
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        _accountRepository.CreateAccount(account);
    }

    public decimal GetBalanceById(string id)
    {
        var balance = _accountRepository.GetBalance(id);

        return balance;
    }
}