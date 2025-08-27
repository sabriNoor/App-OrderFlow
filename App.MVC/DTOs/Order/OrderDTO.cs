using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Order
{
    public record class OrderDTO
    (
        int Id, decimal TotalPrice,List<OrderDetailDTO>OrderDetails
    );
}