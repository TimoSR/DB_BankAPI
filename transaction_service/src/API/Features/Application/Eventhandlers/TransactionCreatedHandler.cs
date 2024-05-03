using API.Features.Domain;
using API.Features.Infrastructure;
using API.Features.Infrastructure.IntegrationEvents;
using MediatR;

namespace API.Features.Application.Eventhandlers;

public class TransactionCreatedHandler : INotificationHandler<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedHandler> _logger;
    private readonly RabbitMQService _rabbitMQService;

    public TransactionCreatedHandler(
        ILogger<TransactionCreatedHandler> logger, 
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
            RequestId = notification.RequestId,
            TransactionId = notification.TransactionId,
            AccountId = notification.AccountId,
            amount = notification.amount,
            CompletionTime = notification.CompletionTime
        };
        
        _rabbitMQService.PublishMessage("transactionEvents", integrationEvent);

        _logger.LogInformation("Published to RabbitMQ: TransactionCreatedEvent");
    }
}