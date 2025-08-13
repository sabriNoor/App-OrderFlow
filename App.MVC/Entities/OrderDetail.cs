using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App.MVC.Entities
{
    [PrimaryKey(nameof(ProductId),nameof(OrderId))]
    public class OrderDetail
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive integer")]
        public int Quantity { get; set; }
        [NotMapped]
        public decimal TotalPrice => Product.Price * Quantity;
        public Product Product { get; set; } = null!;
        public Order Order { get; set; } = null!;
    }
}