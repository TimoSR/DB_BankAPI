namespace API.Features.Domain;

public class TransactionFactory : ITransactionFactory
{
    public Transaction CreateTransaction(Guid commandId, string accountId, decimal amount)
    {
        var id = Guid.NewGuid().ToString();

        var transaction = new Transaction(id, accountId, amount);

        transaction.InitTransaction(commandId);

        return transaction;
    }
}