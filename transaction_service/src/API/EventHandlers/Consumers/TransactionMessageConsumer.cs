using System.Text.Json;
using API.Features.Application;
using API.Features.Infrastructure;

namespace API.EventHandlers.Consumers;

public class TransactionMessageConsumer : BackgroundService
{
    private readonly string _queueName = "transactionEvents";
    private IServiceProvider _services;
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<TransactionMessageConsumer> _logger;

    public TransactionMessageConsumer(RabbitMQService rabbitMQService, ILogger<TransactionMessageConsumer> logger, IServiceProvider services)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
        _services = services;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMQService.StartConsuming(_queueName, ProcessMessage);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }

    private async void ProcessMessage(string message)
    {
        _logger.LogInformation("Processing message: {message}", message);

        using var scope = _services.CreateScope();
        
        var transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
            
        var command = JsonSerializer.Deserialize<CreateTransactionCommand>(message); // Deserialize your message appropriately

        var result = await transactionService.CreateTransactionAsync(command);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation($"Processed message successfully: {command.Id}");
        }
        else
        {
            _logger.LogError($"Failed to process message: {command.Id}");
            // Optionally nack the message or log the error without acknowledging
        }
    }
}