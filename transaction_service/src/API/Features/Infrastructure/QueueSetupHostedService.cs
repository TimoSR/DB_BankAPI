namespace API.Features.Infrastructure;

public class QueueSetupHostedService : IHostedService
{
    private readonly RabbitMQService _rabbitMQService;
    private const string TransactionQue = "transactionEvents";

    public QueueSetupHostedService(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // It is important to make the service that depend on a queue to create itself to not get problems upon startup.
        _rabbitMQService.SetupQueue(TransactionQue);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}