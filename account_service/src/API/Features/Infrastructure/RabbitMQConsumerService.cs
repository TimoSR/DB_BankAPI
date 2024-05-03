using API.Features.Infrastructure;

public class RabbitMQConsumerService : BackgroundService
{
    private readonly RabbitMQService _rabbitMQService;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQConsumerService(RabbitMQService rabbitMQService, ILogger<RabbitMQService> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMQService.StartConsuming("accountEvents", ProcessMessage);
        _rabbitMQService.StartConsuming("transactionEvents", ProcessMessage);
        
        // Keep the task alive as long as the service is not stopped.
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void ProcessMessage(string message)
    {
        _logger.LogInformation("Processing message: {message}", message);
        // Add more processing logic here
    }
}