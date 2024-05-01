namespace API.Features.CreateAccount.Domain;

public interface IAccountRepository
{
    List<Account> GetAccounts();
    Account GetAccount(string id);
    void CreateAccount(Account account);  // Modified to take an Account parameter
    decimal GetBalance(string id);
}

