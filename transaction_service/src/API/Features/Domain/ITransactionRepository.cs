namespace API.Features.Domain;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetTransactionsAsync();
    Task<Transaction?> GetTransactionAsync(string id);
    Task AddTransactionAsync(Guid requestId, Transaction transaction);
    Task<List<Transaction>> GetLast10AccountTransactionsAsync(string accountId);
}

