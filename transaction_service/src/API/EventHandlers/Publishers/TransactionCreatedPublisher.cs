using API.Features.Domain;
using API.Features.Infrastructure;
using MediatR;
using MsgContracts;

namespace API.EventHandlers.Publishers;

public class TransactionCreatedPublisher : INotificationHandler<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedPublisher> _logger;
    private readonly RabbitMQService _rabbitMQService;

    public TransactionCreatedPublisher(
        ILogger<TransactionCreatedPublisher> logger, 
        RabbitMQService rabbitMQService)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
    }

    public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Transaction Created: {notification.Message}");

        var integrationEvent = new TransactionCreatedIntEvent
        {
            RequestId = notification.CommandId,
            TransactionId = notification.TransactionId,
            AccountId = notification.AccountId,
            Amount = notification.amount,
            CompletionTime = notification.CompletionTime
        };
        
        _rabbitMQService.PublishMessage("transactionEvents", integrationEvent);

        _logger.LogInformation("Published to RabbitMQ: TransactionCreatedEvent");
    }
}