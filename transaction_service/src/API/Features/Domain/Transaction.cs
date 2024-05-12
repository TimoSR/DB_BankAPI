using CodeContracts.DDD;
using MsgContracts;

namespace API.Features.Domain;

public class Transaction : AggregateRoot, ITransaction
{
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; init; }
    
    public Transaction(string id, string accountId, decimal amount)
    {
        if (string.IsNullOrEmpty(accountId))
            throw new ArgumentException("Account ID cannot be null or empty.", nameof(accountId));
        if (amount == 0)
            throw new ArgumentException("Amount can't be zero.", nameof(amount));

        Id = id;
        AccountId = accountId;
        Amount = amount;
        Time = DateTime.UtcNow;
    }

    public void InitTransaction(Guid commandId)
    {
        AddDomainEvent(new TransactionCreatedEvent(commandId,Id, AccountId, Amount, Time));
    }
}