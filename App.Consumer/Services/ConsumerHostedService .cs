using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Consumer.Constants;
using App.Consumer.Services.Interfaces;

namespace App.Consumer.Services
{
    public class ConsumerHostedService : IHostedService
    {
        private readonly IRabbitMQConsumer _consumer;

        public ConsumerHostedService(IRabbitMQConsumer consumer)
        {
            _consumer = consumer;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _consumer.ConsumeAsync(RabbitMQQueues.OrderQueue, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}