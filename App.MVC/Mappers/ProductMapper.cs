using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs.Product;
using App.MVC.Entities;

namespace App.MVC.Mappers
{
    public static class ProductMapper
    {
        public static Product ToModel(this CreateUpdateProductDTO dto)
        {
            return new Product
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price
            };
        }

        public static ProductDTO ToDTO(this Product product)
        {
            return new ProductDTO()
            {
                Id=product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }
    }
}