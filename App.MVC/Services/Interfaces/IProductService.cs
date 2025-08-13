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
        Task<ServiceResult<Product>> CreateProduct(CreateUpdateProductDTO productDTO);
        Task<ServiceResult<Product>> UpdateProduct(int id, CreateUpdateProductDTO productDTO);
        Task<ServiceResult<string>> DeleteProduct(int id);
        Task<ServiceResult<IEnumerable<Product>>> GetAllProduct();
        Task<ServiceResult<Product>> GetProductById(int id);
    }
}