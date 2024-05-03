using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public interface IAccountService
{
    Task<ServiceResult<List<AccountDTO>>> GetAllAccountsAsync();
    Task<ServiceResult<AccountDTO>> GetAccountByIdAsync(string id);
    Task<ServiceResult<decimal>> GetBalanceByIdAsync(string id);
    Task<ServiceResult> CreateAccountAsync(CreateAccountRequest request);
    Task<ServiceResult> UpdateAccountBalanceAsync(UpdateBalanceRequest request);
}