using API.Features.Domain;
using API.Features.Infrastructure.Contexts;
using CodeContracts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionContext _context;
    private IDomainEventDispatcher _dispatcher;

    public TransactionRepository(
        TransactionContext context,
        IDomainEventDispatcher dispatcher)
    {
        _context = context;
        _dispatcher = dispatcher;
    }

    // Asynchronous method to retrieve all accounts
    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        return await _context.Transactions.ToListAsync();
    }

    // Asynchronous method to retrieve a single account by ID
    public async Task<Transaction?> GetTransactionAsync(string id)
    {
        var result = await _context.Transactions.FindAsync(id);

        return result;
    }
    
    public async Task CreateTransactionAsync(Guid requestId, Transaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }
        
        transaction.CreateTransaction(requestId);
        
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        await _dispatcher.DispatchEventsAsync(transaction);
    }
}