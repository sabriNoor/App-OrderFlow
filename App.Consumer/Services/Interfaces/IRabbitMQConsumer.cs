using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Consumer.Services.Interfaces
{
    public interface IRabbitMQConsumer
    {
        Task ConsumeAsync(string queueName, CancellationToken stoppingToken);
    }
}