namespace API.Features.Domain;

public interface ITransactionFactory
{
    Transaction CreateTransaction(Guid commandId, string accountId, decimal amount);
}