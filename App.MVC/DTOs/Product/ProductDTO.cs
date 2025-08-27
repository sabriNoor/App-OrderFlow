using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Product
{
    public record class ProductDTO
    {
        public int Id{ get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Quantity { get; init; }
    }
}