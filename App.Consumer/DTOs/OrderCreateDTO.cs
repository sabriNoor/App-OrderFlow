using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Consumer.DTOs
{
    public record class OrderCreateDTO
    {
        public int Id { get; init; }
        public decimal TotalPrice { get; init; }

        public List<OrderDetailsDTO> OrderDetails { get; init; } = [];

    }
    public record class OrderDetailsDTO
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;
        public int Quantity { get; init; }
        public decimal TotalPrice{ get; init; }
    }

    
}