using API.Features.CreateAccount.Domain;

namespace API.Features.Application;

public interface IAccountService
{
    List<AccountDto> GetAllAccounts();
    AccountDto GetAccountById(string id);
    void CreateAccount(CreateAccountRequest request);
    decimal GetBalanceById(string id);
}