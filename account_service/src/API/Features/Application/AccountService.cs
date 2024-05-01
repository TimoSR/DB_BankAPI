using API.Features.CreateAccount.Domain;
using CodeContracts.Application.ServiceResultPattern;

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
    
    public ServiceResult<List<AccountDto>> GetAllAccounts()
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

        return ServiceResult<List<AccountDto>>.Success(accountDTOs);
    }

    public ServiceResult<AccountDto> GetAccountById(string id)
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
        
        return ServiceResult<AccountDto>.Success(accountDto);
    }

    public ServiceResult CreateAccount(CreateAccountRequest request)
    {
        var account = new Account()
        {
            CPR = _security.Hash(request.CPR),
            FirstName = request.FirstName,
            LastName = request.LastName
        };
        
        _accountRepository.CreateAccount(account);

        return ServiceResult.Success("Account Created Successfully!");
    }

    public ServiceResult<decimal> GetBalanceById(string id)
    {
        var balance = _accountRepository.GetBalance(id);

        return ServiceResult<decimal>.Success(balance);
    }
}