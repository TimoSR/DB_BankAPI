namespace API.Features.Infrastructure.IntegrationEvents;

public readonly record struct TransactionCreatedIntEvent(Guid RequestId, string TransactionId, string AccountId, decimal Amount, DateTime CompletionTime);