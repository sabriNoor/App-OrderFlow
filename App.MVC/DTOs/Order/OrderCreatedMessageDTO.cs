using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Order
{
    public record class OrderCreatedMessageDTO
    {
        public OrderDTO Order { get; set; } = null!;
        public string Email { get; set; } = string.Empty;

    }
}