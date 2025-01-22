using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct AccountCreatedEvent(string AccountId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Created Account {AccountId} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

public readonly record struct BalanceUpdatedEvent(string AccountId, decimal Amount, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Updated account {AccountId} with amount {Amount} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";

}