namespace MsgContracts;

public readonly record struct TransactionCreatedIntEvent(Guid RequestId, string TransactionId, string AccountId, decimal Amount, DateTime CompletionTime);