using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct AccountCreatedEvent(string AccountId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Account {AccountId} created at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}