using API.Features.Domain;
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
    
    public async Task<ServiceResult<List<AccountDTO>>> GetAllAccountsAsync()
    {
        try
        {
            var accounts = await _accountRepository.GetAccountsAsync();
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

    public async Task<ServiceResult<AccountDTO>> GetAccountByIdAsync(string id)
    {
        try
        {
            var account = await _accountRepository.GetAccountAsync(id);
        
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

    public async Task<ServiceResult> CreateAccountAsync(CreateAccountRequest request)
    {
        try
        {
            var account = new Account()
            {
                CPR = _security.Hash(request.CPR),
                FirstName = request.FirstName,
                LastName = request.LastName
            };
        
            await _accountRepository.CreateAccountAsync(account);

            return ServiceResult.Success("Account Created Successfully!");
        }
        catch
        {
            return ServiceResult.Failure("Failed to create account.");
        }
    }

    public async Task<ServiceResult<decimal>> GetBalanceByIdAsync(string id)
    {
        try
        {
            var balance = await _accountRepository.GetBalanceAsync(id);

            return ServiceResult<decimal>.Success(balance);
        }
        catch
        {
            return ServiceResult<decimal>.Failure("Failed to get balance.");
        }
    }
}