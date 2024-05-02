using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace API.Features.Infrastructure;

public class RabbitMQService
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;

    public RabbitMQService(IConfiguration config, ILogger<RabbitMQService> logger)
    {
        _logger = logger;
        
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
        _channel.QueueDeclare(
            queue: queueName,
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
    
    public void StartConsuming(string queueName, Action<string> processMessageAction)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Received message: {message}", message);

            // Invoke the passed action to process the message
            processMessageAction(message);

            // Acknowledge the message as processed
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }
}