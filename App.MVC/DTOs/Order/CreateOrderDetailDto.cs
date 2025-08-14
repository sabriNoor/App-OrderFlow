using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Order
{
    public record class CreateOrderDetailDto
    {
        public int ProductId { get; init; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive integer")]
        public int Quantity { get; init; }
        
    }
    
}