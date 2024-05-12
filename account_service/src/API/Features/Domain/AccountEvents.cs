using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct AccountCreatedEvent(Guid CommandId, string AccountId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Request {CommandId} Created Account {AccountId} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

public readonly record struct BalanceUpdatedEvent(
    Guid CommandId,
    string AccountId,
    decimal Amount,
    DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Request {CommandId} updated account {AccountId} with amount {Amount} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";

}