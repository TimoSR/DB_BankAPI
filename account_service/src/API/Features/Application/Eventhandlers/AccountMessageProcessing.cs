using API.Features.Infrastructure;

namespace API.Features.Application.Eventhandlers;

public class AccountMessageProcessing
{
    private readonly RabbitMQService _rabbitMQService;

    public AccountMessageProcessing(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public void Start()
    {
        _rabbitMQService.StartConsuming("accountEvents", ProcessMessage);
    }

    private void ProcessMessage(string message)
    {
        
    }
}