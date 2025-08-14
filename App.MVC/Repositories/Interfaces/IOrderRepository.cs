using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.MVC.DTOs.Order;
using App.MVC.Entities;

namespace App.MVC.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<List<OrderDTO>> GetOrdersInformation(Expression<Func<Order, bool>> func);
        
    }
}