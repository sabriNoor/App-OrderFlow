using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs;
using App.MVC.DTOs.Order;
using App.MVC.Entities;

namespace App.MVC.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<OrderDTO>> CreateOrder(int userId,string email, CreateOrderDTO orderDTO);
        Task<ServiceResult<List<OrderDTO>>> GetMyOrders(int userId);
        Task<ServiceResult<OrderDTO>> GetOrderByIdForUserAsync(int id, int userId);
        Task<ServiceResult<OrderDTO>> GetOrderByIdForAdminAsync(int id);
    }
}