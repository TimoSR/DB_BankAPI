using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct TransactionCreatedEvent(Guid RequestId, string TransactionId, string AccountId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Request {RequestId} Created transaction {TransactionId} for account {AccountId} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}