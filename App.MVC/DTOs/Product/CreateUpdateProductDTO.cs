using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Product
{
    public record class CreateUpdateProductDTO
    {
        [Required (ErrorMessage = "Product name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Length should be between 3 and 50 characters")]
        public string Name { get; init; } = string.Empty;

        [Required (ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be positive integer")]
        public int Quantity { get; init; }
        
    }
}