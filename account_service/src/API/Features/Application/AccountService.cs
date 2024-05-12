using API.Features.Domain;
using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountSecurityDomainService _security;
    private readonly ILogger<AccountService> _logger;
    private readonly IAccountFactory _factory;

    public AccountService( 
        IAccountRepository accountRepository,
        IAccountFactory factory,
        IAccountSecurityDomainService security,
        ILogger<AccountService> logger)
    {
        _accountRepository = accountRepository;
        _factory = factory;
        _security = security;
        _logger = logger;
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
                    Id = account.ID,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Balance = account.Balance
                });
            }
            return ServiceResult<List<AccountDTO>>.Success(accountDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get accounts.");
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
                Id = account.ID,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Balance = account.Balance
            };
        
            return ServiceResult<AccountDTO>.Success(accountDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to get account with ID {id}", id);
            return ServiceResult<AccountDTO>.Failure($"Failed to get account with {id}");
        }
    }

    public async Task<ServiceResult<decimal>> GetBalanceByIdAsync(string id)
    {
        try
        {
            var balance = await _accountRepository.GetBalanceAsync(id);

            return ServiceResult<decimal>.Success(balance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to get balance of account {id}", id);
            return ServiceResult<decimal>.Failure($"Failed to get balance of account {id}.");
        }
    }
    
    public async Task<ServiceResult> CreateAccountAsync(CreateAccountCommand command)
    {
        try
        {
            var hashedCPR = _security.Hash(command.CPR);
            
            var account = _factory.CreateAccount(command.Id, hashedCPR, command.FirstName, command.LastName);
            
            await _accountRepository.CreateAccountAsync(command.Id, account);
            
            _logger.LogInformation("Request {RequestId} successfully created an account!", command.Id);
            return ServiceResult.Success($"Account {account.ID} Created Successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Request {RequestId} failed to create account", command.Id);
            return ServiceResult.Failure("Failed to create account.");
        }
    }

    public async Task<ServiceResult> UpdateAccountBalanceAsync(UpdateBalanceCommand command)
    {
        try
        {
            await _accountRepository.UpdateBalanceAsync(command.Id, command.AccountId, command.Amount);
            return ServiceResult.Success($"Account {command.AccountId} balance updated {command.Amount} successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Request {RequestId} failed to update account balance", command.Id);
            return ServiceResult.Failure("Failed to update account balance.");
        }
    }
}