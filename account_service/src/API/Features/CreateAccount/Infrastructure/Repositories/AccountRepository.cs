using API.Features.CreateAccount.Domain;
using API.Features.CreateAccount.Infrastructure.Contexts;

namespace API.Features.CreateAccount.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _context;

    public AccountRepository(AccountContext context)
    {
        _context = context;
    }

    public List<Account> GetAccounts()
    {
        return _context.Accounts.ToList();
    }

    public Account GetAccount(string id)
    {
        if (!int.TryParse(id, out var accountId))
        {
            throw new ArgumentException("Invalid account ID format", nameof(id));
        }
        return _context.Accounts.Find(accountId);
    }

    public void CreateAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account));
        }
        _context.Accounts.Add(account);
        _context.SaveChanges();
    }

    public decimal GetBalance(string id)
    {
        var account = GetAccount(id);
        if (account == null)
        {
            throw new InvalidOperationException("Account not found");
        }
        return account.Balance;
    }
}