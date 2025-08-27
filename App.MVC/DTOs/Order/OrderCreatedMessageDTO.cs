using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Order
{
    public record class OrderCreatedMessageDTO
    {
        public string Email { get; init; } = string.Empty;
        public string Subject { get; init; } = string.Empty;

        public string Body { get; init; } = string.Empty;


    }
}