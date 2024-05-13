using CodeContracts.DDD;

namespace API.Features.Domain;

public readonly record struct TransactionCreatedEvent(Guid CommandId, string TransactionId, string AccountId, decimal Amount, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Command {CommandId} Created transaction {TransactionId} for account {AccountId} with {Amount} at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}