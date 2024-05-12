namespace API.Features.Infrastructure;

public class QueueSetupHostedService(RabbitMQService rabbitMQService) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Set up all necessary queues
        rabbitMQService.SetupQueue("transactionEvents");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
