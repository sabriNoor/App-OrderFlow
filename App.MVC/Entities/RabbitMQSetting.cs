using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.Entities
{
    public class RabbitMQSetting
    {
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        
    }
}