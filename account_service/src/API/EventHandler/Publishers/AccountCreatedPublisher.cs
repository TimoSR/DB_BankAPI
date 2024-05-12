using API.Features.Domain;
using API.Features.Infrastructure;
using MediatR;

namespace API.EventHandler.Publishers;

public class AccountCreatedPublisher : INotificationHandler<AccountCreatedEvent>
{
    private readonly ILogger<AccountCreatedPublisher> _logger;
    private readonly RabbitMQService _rabbitMQService;

    public AccountCreatedPublisher(
        ILogger<AccountCreatedPublisher> logger, 
        RabbitMQService rabbitMQService)
    {
        _logger = logger;
        _rabbitMQService = rabbitMQService;
    }

    public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Account Created: {notification.Message}");

        var integrationEvent = new
        {
            RequestId = notification.RequestId,
            AuctionId = notification.AccountId,
            CompletionTime = notification.CompletionTime
        };
        
        _rabbitMQService.PublishMessage("accountEvents", integrationEvent);

        _logger.LogInformation("Published to RabbitMQ: AccountCreatedEvent");
    }
}