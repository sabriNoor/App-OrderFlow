using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.MVC.Constants;
using App.MVC.DTOs.Product;
using App.MVC.Entities;
using App.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateUpdateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.CreateProductAsync(productDTO);
            if (result.Success)
                return CreatedAtAction(nameof(GetProductById), new { id = result?.Data?.Id }, result?.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpPut("{id}")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> UpdateProduct(int id, CreateUpdateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateProductAsync(id, productDTO);
            if (result.Success)
                return Ok(result.Data);

            return NotFound(new { Error = result.ErrorMessage });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles=Roles.Admin)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (result.Success)
                return Ok(new { Message = result.Data });

            return NotFound(new { Error = result.ErrorMessage });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAllProductAsync();
            if (result.Success)
                return Ok(result.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            if (result.Success)
                return Ok(result.Data);

            return NotFound(new { Error = result.ErrorMessage });
        }
    }
}
