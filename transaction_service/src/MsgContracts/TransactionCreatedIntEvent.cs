namespace MsgContracts;

public readonly record struct TransactionCreatedIntEvent(Guid CommandId, string TransactionId, string AccountId, decimal Amount, DateTime CompletionTime);