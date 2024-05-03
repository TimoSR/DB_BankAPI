using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct TransactionCreatedEvent(Guid RequestId, string AccountId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Request {RequestId} Created Account {AccountId} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}