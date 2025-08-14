using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Data;
using App.MVC.DTOs.Product;
using App.MVC.Entities;
using App.MVC.Mappers;
using App.MVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.MVC.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            return await _dbContext.Products.Select(p => p.ToDTO()).ToListAsync();
        }
    }
}