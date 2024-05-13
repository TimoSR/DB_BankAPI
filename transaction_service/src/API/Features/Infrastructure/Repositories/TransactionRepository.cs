using API.Features.Domain;
using API.Features.Infrastructure.Contexts;
using CodeContracts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Infrastructure.Repositories;

public class TransactionRepository(TransactionContext context, IDomainEventDispatcher dispatcher) : ITransactionRepository
{
    // Asynchronous method to retrieve all accounts
    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        return await context.Transactions.ToListAsync();
    }

    // Asynchronous method to retrieve a single account by ID
    public async Task<Transaction?> GetTransactionAsync(string id)
    {
        var result = await context.Transactions.FindAsync(id);
        return result;
    }
    
    public async Task<List<Transaction>> GetLast10TransactionsAsync(string accountId)
    {
        return await context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderByDescending(t => t.Time) 
            .Take(10)
            .ToListAsync();
    }
    
    public async Task AddTransactionAsync(Guid requestId, Transaction transaction)
    {
        await context.Transactions.AddAsync(transaction);
        await context.SaveChangesAsync();
        await dispatcher.DispatchEventsAsync(transaction);
    }
}