using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace API.Features.Infrastructure;

using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

public class RabbitMQService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;
    private bool _disposed;

    public RabbitMQService(ConnectionFactory factory, ILogger<RabbitMQService> logger)
    {
        _logger = logger;
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void SetupQueue(string queueName)
    {
        try
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to declare queue: {ex.Message}");
            throw;
        }
    }

    public void StartConsuming(string queueName, Action<string> processMessageAction)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {message}", message);
                processMessageAction(message);
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing message: {ex.Message}");
                // Consider handling error properly or requeuing the message
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void PublishMessage<T>(string routingKey, T message)
    {
        try
        {
            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            _channel.BasicPublish(
                exchange: "",
                routingKey: routingKey,
                basicProperties: null,
                body: messageBody);
            _logger.LogInformation("Published message to {routingKey}", routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to publish message: {ex.Message}");
            throw;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _channel?.Close();
                _connection?.Close();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}