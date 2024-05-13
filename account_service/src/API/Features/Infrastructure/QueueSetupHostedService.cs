namespace API.Features.Infrastructure;

public class QueueSetupHostedService : IHostedService
{
    private readonly RabbitMQService _rabbitMQService;

    public QueueSetupHostedService(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rabbitMQService.SetupQueue("accountEvents");
        _rabbitMQService.SetupQueue("transactionEvents");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
