using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.MVC.Data;
using App.MVC.DTOs.Order;
using App.MVC.Entities;
using App.MVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace App.MVC.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }

        public async Task<List<OrderDTO>> GetOrdersInformation(Expression<Func<Order,bool>>  func)
        {
            var order =await _dbContext.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(func)
                .Select(o => new OrderDTO
                (
                    o.Id,
                    o.TotalPrice,
                    o.OrderDetails.Select(od => new OrderDetailDTO
                    (
                        od.ProductId,
                        od.Product.Name,
                        od.Quantity,
                        od.TotalPrice
                    )).ToList()
                )).ToListAsync();
                
            return order??[];

        }

    }
}