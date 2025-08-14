using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs;
using App.MVC.DTOs.Product;
using App.MVC.Entities;

namespace App.MVC.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<Product>> CreateProductAsync(CreateUpdateProductDTO productDTO);
        Task<ServiceResult<Product>> UpdateProductAsync(int id, CreateUpdateProductDTO productDTO);
        Task<ServiceResult<string>> DeleteProductAsync(int id);
        Task<ServiceResult<IEnumerable<Product>>> GetAllProductAsync();
        Task<ServiceResult<Product>> GetProductByIdAsync(int id);
    }
}