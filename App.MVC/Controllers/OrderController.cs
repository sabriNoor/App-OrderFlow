using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.MVC.Constants;
using App.MVC.DTOs.Order;
using App.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _orderService.CreateOrder(userId, orderDTO);
            if (result.Success)
                return CreatedAtAction(nameof(GetOrderByIdForUser), new { id = result?.Data?.Id }, result?.Data);

            return BadRequest(new { Error = result.ErrorMessage });
        }
        [HttpGet("{id}")]
        [Authorize(Roles =Roles.User)]
        public async Task<IActionResult> GetOrderByIdForUser(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _orderService.GetOrderByIdForUserAsync(id, userId);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);
        }
        [HttpGet("admin/{id}")]
        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> GetOrderByIdForAdmin(int id)
        {
            var result = await _orderService.GetOrderByIdForAdminAsync(id);
            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.ErrorMessage);
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _orderService.GetMyOrders(userId);
            if (result.Success)
                return Ok(result.Data);

            return StatusCode(500, result.ErrorMessage);
        }
    }
}