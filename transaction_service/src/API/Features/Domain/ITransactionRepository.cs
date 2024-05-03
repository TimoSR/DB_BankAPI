namespace API.Features.Domain;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetTransactionsAsync();
    Task<Transaction?> GetTransactionAsync(string id);
    Task CreateTransactionAsync(Guid requestId, Transaction transaction);
}

