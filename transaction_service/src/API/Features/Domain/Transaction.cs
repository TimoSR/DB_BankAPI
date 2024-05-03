using CodeContracts.DDD;

namespace API.Features.Domain;

public class Transaction : AggregateRoot, ITransaction
{
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; private set; }
    
    public void CreateTransaction(Guid requestId)
    {
        Time = DateTime.Now;
        
        AddDomainEvent(new TransactionCreatedEvent(requestId, Id, Time));
    }
}