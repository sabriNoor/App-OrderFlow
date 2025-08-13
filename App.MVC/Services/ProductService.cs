using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs;
using App.MVC.DTOs.Product;
using App.MVC.Entities;
using App.MVC.Mappers;
using App.MVC.Repositories.Interfaces;
using App.MVC.Services.Interfaces;

namespace App.MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ServiceResult<Product>> CreateProduct(CreateUpdateProductDTO productDTO)
        {
            try
            {
                var product = productDTO.ToModel();
                await _productRepository.AddAsync(product);
                _logger.LogInformation("Product created successfully: {ProductName} (ID: {ProductId})", product.Name, product.Id);

                return ServiceResult<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product: {ProductName}", productDTO.Name);
                return ServiceResult<Product>.Fail("An unexpected error occurred while saving the product. Please try again.");
            }
        }

        public async Task<ServiceResult<Product>> UpdateProduct(int id, CreateUpdateProductDTO productDTO)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    _logger.LogWarning("Update failed: Product with ID {ProductId} not found", id);
                    return ServiceResult<Product>.Fail("Product not found.");
                }

                var originalProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity
                };

                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.Quantity = productDTO.Quantity;

                await _productRepository.UpdateAsync(product);

                _logger.LogInformation(
                    "Product updated successfully: {ProductName} (ID: {ProductId}). Changes: Name: '{OldName}' → '{NewName}', Price: {OldPrice} → {NewPrice}, Quantity: {OldQty} → {NewQty}",
                    product.Name, product.Id,
                    originalProduct.Name, product.Name,
                    originalProduct.Price, product.Price,
                    originalProduct.Quantity, product.Quantity
                );

                return ServiceResult<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product: {ProductName}", productDTO.Name);
                return ServiceResult<Product>.Fail("An unexpected error occurred while updating the product. Please try again.");
            }
        }


        public async Task<ServiceResult<string>> DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    _logger.LogWarning("Delete failed: Product with ID {ProductId} not found", id);
                    return ServiceResult<string>.Fail("Product not found.");
                }

                await _productRepository.DeleteAsync(product);
                _logger.LogInformation("Product deleted successfully: {ProductName} (ID: {ProductId})", product.Name, product.Id);

                return ServiceResult<string>.Ok("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID {ProductId}", id);
                return ServiceResult<string>.Fail("An unexpected error occurred while deleting the product. Please try again.");
            }
        }

        public async Task<ServiceResult<IEnumerable<Product>>> GetAllProduct()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                _logger.LogInformation("Fetched {Count} products from database.", products.Count());

                return ServiceResult<IEnumerable<Product>>.Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                return ServiceResult<IEnumerable<Product>>.Fail("An unexpected error occurred while retrieving products. Please try again.");
            }
        }

        public async Task<ServiceResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product is null)
                {
                    _logger.LogWarning("Get product failed: Product with ID {ProductId} not found", id);
                    return ServiceResult<Product>.Fail("Product not found.");
                }

                _logger.LogInformation("Fetched product successfully: {ProductName} (ID: {ProductId})", product.Name, product.Id);
                return ServiceResult<Product>.Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with ID {ProductId}", id);
                return ServiceResult<Product>.Fail("An unexpected error occurred while retrieving the product. Please try again.");
            }
        }
    }
}
