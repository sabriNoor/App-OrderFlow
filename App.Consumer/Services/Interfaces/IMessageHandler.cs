using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Consumer.Services.Interfaces
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(string message);
    }
}