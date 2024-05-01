using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public interface IAccountService
{
    ServiceResult<List<AccountDto>> GetAllAccounts();
    ServiceResult<AccountDto> GetAccountById(string id);
    ServiceResult<decimal> GetBalanceById(string id);
    ServiceResult CreateAccount(CreateAccountRequest request);
}