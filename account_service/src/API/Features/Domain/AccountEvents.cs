using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct AccountCreatedEvent(Guid RequestId, string AccountId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Request {RequestId} Created Account {AccountId} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

public readonly record struct BalanceUpdatedEvent(
    Guid RequestId,
    string AccountId,
    decimal Amount,
    DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Request {RequestId} updated account {AccountId} with amount {Amount} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";

}