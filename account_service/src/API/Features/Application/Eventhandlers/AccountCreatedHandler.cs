using System.Text;
using System.Text.Json;
using API.Features.Domain;
using API.Features.Infrastructure;
using MediatR;
using RabbitMQ.Client;

namespace API.Features.Application.Eventhandlers;

public class AccountCreatedHandler : INotificationHandler<AccountCreatedEvent>
{
    private readonly ILogger<AccountCreatedHandler> _logger;
    private readonly RabbitMQService _rabbitMQService;

    public AccountCreatedHandler(
        ILogger<AccountCreatedHandler> logger, 
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