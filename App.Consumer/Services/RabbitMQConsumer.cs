using System.Text;
using App.Consumer.Models;
using App.Consumer.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace App.Consumer.Services
{
    public class RabbitMQConsumer : IAsyncDisposable, IRabbitMQConsumer
    {
        private readonly ILogger<RabbitMQConsumer> _logger;
        private readonly RabbitMQSetting _rabbitMqSetting;
        private readonly IMessageHandler _messageHandler;
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQConsumer(RabbitMQSetting rabbitMqSetting, ILogger<RabbitMQConsumer> logger, IMessageHandler messageHandler)
        {
            _rabbitMqSetting = rabbitMqSetting;
            _logger = logger;
            _messageHandler = messageHandler;
        }

        private async Task InitRabbitMqAsync()
        {
            if (_connection != null && _channel != null)
                return;

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation("RabbitMQ consumer connection & channel created");
        }

        public async Task ConsumeAsync(string queueName, CancellationToken stoppingToken)
        {
            try
            {
                await InitRabbitMqAsync();

                if (_channel is null)
                    throw new InvalidOperationException("RabbitMQ is not initialized.");

                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += OnMessageReceived;

                await _channel.BasicConsumeAsync(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken);

                _logger.LogInformation("Started consuming queue {Queue}", queueName);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Stopping consumer for queue {Queue}", queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while consuming queue {Queue}", queueName);
                throw;
            }
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            await _messageHandler.HandleMessageAsync(message);
            Console.WriteLine($"Received: {message}");

            await _channel!.BasicAckAsync(e.DeliveryTag, multiple: false);
        }


        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
                await _channel.DisposeAsync();
            if (_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}
