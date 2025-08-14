using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs.Order
{
    public record class OrderDetailDTO
    (
        int ProductId,
        string ProductName,
        int Quantity,
        decimal TotalPrice
    );
}