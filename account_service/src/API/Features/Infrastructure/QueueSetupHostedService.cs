namespace API.Features.Infrastructure;

public class QueueSetupHostedService : IHostedService
{
    private readonly RabbitMQService _rabbitMQService;
    private const string AccountQue = "accountEvents";
    private const string TransactionQue = "transactionEvents";
    
    public QueueSetupHostedService(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _rabbitMQService.SetupQueue(AccountQue);
        _rabbitMQService.SetupQueue(TransactionQue);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
