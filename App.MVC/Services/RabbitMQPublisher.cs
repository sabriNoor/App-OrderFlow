using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.MVC.Entities;
using App.MVC.Services.Interfaces;
using RabbitMQ.Client;
using Newtonsoft.Json;

namespace App.MVC.Services
{
    public class RabbitMQPublisher<T> : IRabbitMQPublisher<T> , IAsyncDisposable
    {
        private readonly RabbitMQSetting _rabbitMQSetting;
        private readonly ILogger<RabbitMQPublisher<T>> _logger;
        private IConnection? _connection;
        private IChannel? _channel;
        public RabbitMQPublisher(RabbitMQSetting rabbitMQSetting, ILogger<RabbitMQPublisher<T>> logger)
        {
            _rabbitMQSetting = rabbitMQSetting;
            _logger = logger;

        }

        public async Task PublishMessageAsync(T message, string queueName)
        {
            try
            {
                if (_channel is null)
                {
                    throw new InvalidOperationException("RabbitMQ is not initialized. Call InitializeAsync() first.");
                }
                

                _logger.LogInformation("Declaring queue {QueueName}", queueName);

                await _channel.QueueDeclareAsync(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var messageJson = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageJson);

                var properties = new BasicProperties() { Persistent = false };

                await _channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: queueName,
                    mandatory: false,
                    basicProperties: properties,
                    body: new ReadOnlyMemory<byte>(body));

                _logger.LogInformation("Message published to queue {QueueName}: {Message}", queueName, messageJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish message to queue {QueueName}", queueName);
                throw;
            }
        }

        public async Task InitializeAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQSetting.HostName,
                UserName = _rabbitMQSetting.UserName,
                Password = _rabbitMQSetting.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation("RabbitMQ connection & channel created");
        }

        public async ValueTask DisposeAsync()
        {
            if(_channel is not null )
                await _channel.DisposeAsync();
            if(_connection is not null)
                await _connection.DisposeAsync();
        }
    }
}