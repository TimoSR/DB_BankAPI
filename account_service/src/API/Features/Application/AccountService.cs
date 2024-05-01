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
    
    public ServiceResult<List<AccountDTO>> GetAllAccounts()
    {
        try
        {
            var accounts = _accountRepository.GetAccounts();
            var accountDTOs = new List<AccountDTO>(); 
            
            foreach (var account in accounts)
            {
                accountDTOs.Add(new AccountDTO()
                {
                    Id = account.Id,
                    CPR = account.CPR,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Balance = account.Balance
                });
            }
            
            return ServiceResult<List<AccountDTO>>.Success(accountDTOs);
        }
        catch
        {
            return ServiceResult<List<AccountDTO>>.Failure("Failed to get accounts.");
        }
    }

    public ServiceResult<AccountDTO> GetAccountById(string id)
    {
        try
        {
            var account = _accountRepository.GetAccount(id);
        
            var accountDTO = new AccountDTO()
            {
                Id = account.Id,
                CPR = account.CPR,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Balance = account.Balance
            };
        
            return ServiceResult<AccountDTO>.Success(accountDTO);
        }
        catch
        {
            return ServiceResult<AccountDTO>.Failure($"Failed to get account with ID: {id}");
        }
    }

    public ServiceResult CreateAccount(CreateAccountRequest request)
    {
        try
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
        catch
        {
            return ServiceResult.Failure("Failed to create account.");
        }
    }

    public ServiceResult<decimal> GetBalanceById(string id)
    {
        try
        {
            var balance = _accountRepository.GetBalance(id);

            return ServiceResult<decimal>.Success(balance);
        }
        catch
        {
            return ServiceResult<decimal>.Failure("Failed to get balance.");
        }
    }
}