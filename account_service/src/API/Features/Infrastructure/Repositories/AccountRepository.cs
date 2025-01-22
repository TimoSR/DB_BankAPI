using API.Features.Domain;
using API.Features.Infrastructure.Contexts;
using CodeContracts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Infrastructure.Repositories;

public class AccountRepository(AccountContext context, IDomainEventDispatcher dispatcher) : IAccountRepository
{
    
    // Domain Event Dispatcher should be orchestrated at the application layer
    
    // Queries
    
    public async Task<Account?> GetAccountAsync(string id)
    {
        var result = await context.Accounts.FindAsync(id);
        
        if (result == null)
        {
            throw new InvalidOperationException("Account not found");
        }
        
        return result;
    }
    
    public async Task<List<Account>> GetAccountsAsync()
    {
        return await context.Accounts.ToListAsync();
    }
    
    public async Task<decimal> GetBalanceAsync(string id)
    {
        var account = await GetAccountAsync(id);
        
        if (account == null)
        {
            throw new InvalidOperationException("Account not found");
        }
        
        return account.Balance;
    }
    
    // Commands
    
    public async Task CreateAccountAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
        await dispatcher.DispatchEventsAsync(account);
    }
    
    // This should be orchestrated in the application layer
    public async Task UpdateBalanceAsync(string id, decimal amount)
    {
        var account = await context.Accounts.FindAsync(id);

        if (account == null)
        {
            throw new InvalidOperationException("Account not found");
        }

        account.UpdateBalance(amount);
        await context.SaveChangesAsync();
        await dispatcher.DispatchEventsAsync(account);
    }
}