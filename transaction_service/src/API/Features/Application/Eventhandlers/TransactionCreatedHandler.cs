using API.Features.Domain;
using API.Features.Infrastructure;
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

        var integrationEvent = new
        {
            RequestId = notification.RequestId,
            AuctionId = notification.AccountId,
            CompletionTime = notification.CompletionTime
        };
        
        _rabbitMQService.PublishMessage("transactionEvents", integrationEvent);

        _logger.LogInformation("Published to RabbitMQ: TransactionCreatedEvent");
    }
}