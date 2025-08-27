using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs.Product;
using App.MVC.Entities;

namespace App.MVC.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
    }
}