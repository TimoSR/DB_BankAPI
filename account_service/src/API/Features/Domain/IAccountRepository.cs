namespace API.Features.Domain;

public interface IAccountRepository
{
    Task<List<Account>> GetAccountsAsync();
    Task<Account> GetAccountAsync(string id);
    Task CreateAccountAsync(Account account);  // Modified to take an Account parameter
    Task<decimal> GetBalanceAsync(string id);
}

