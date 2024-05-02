using API.Features.Domain;
using API.Features.Infrastructure.Contexts;
using CodeContracts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountContext _context;
    private IDomainEventDispatcher _dispatcher;

    public AccountRepository(
        AccountContext context,
        IDomainEventDispatcher dispatcher)
    {
        _context = context;
        _dispatcher = dispatcher;
    }

    // Asynchronous method to retrieve all accounts
    public async Task<List<Account>> GetAccountsAsync()
    {
        return await _context.Accounts.ToListAsync();
    }

    // Asynchronous method to retrieve a single account by ID
    public async Task<Account> GetAccountAsync(string id)
    {
        return await _context.Accounts.FindAsync(id);
    }

    // Asynchronous method to retrieve the balance of a specific account
    public async Task<decimal> GetBalanceAsync(string id)
    {
        var account = await GetAccountAsync(id);
        if (account == null)
        {
            throw new InvalidOperationException("Account not found");
        }
        return account.Balance;
    }
    
    // Asynchronous method to create a new account
    public async Task CreateAccountAsync(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account));
        }
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();
        await _dispatcher.DispatchEventsAsync(account);
    }
}