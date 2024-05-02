using RabbitMQ.Client;

namespace API.Features.Infrastructure;

public class RabbitMQService
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQService(IConfiguration config)
    {
        var rabbitMQConfig = config.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQConfig.GetValue<string>("HostName"),
            Port = rabbitMQConfig.GetValue<int>("Port"),
            UserName = rabbitMQConfig.GetValue<string>("UserName"),
            Password = rabbitMQConfig.GetValue<string>("Password")
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }
    
    public IModel GetChannel()
    {
        return _channel;
    }

    public void SetupQueue(string queueName)
    {
        _channel.QueueDeclare(queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    // Ensure proper disposal of connections and channels
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}