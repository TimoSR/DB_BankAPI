using API.Features.Infrastructure;

public class RabbitMQConsumerService : BackgroundService
{
    private readonly RabbitMQService _rabbitMQService;

    public RabbitMQConsumerService(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _rabbitMQService.StartConsuming("accountEvents", ProcessMessage);
        
        // Keep the task alive as long as the service is not stopped.
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void ProcessMessage(string message)
    {
        Console.WriteLine($"Processing message: {message}");
        // Add more processing logic here
    }
}