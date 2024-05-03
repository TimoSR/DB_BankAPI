namespace API.Features.Domain;

public interface IAccountRepository
{
    Task<List<Account>> GetAccountsAsync();
    Task<Account?> GetAccountAsync(string id);
    Task CreateAccountAsync(Guid requestId, Account account);
    Task UpdateBalanceAsync(Guid requestId, string id, decimal amount);
    Task<decimal> GetBalanceAsync(string id);
}

