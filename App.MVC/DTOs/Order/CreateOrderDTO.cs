using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Entities;

namespace App.MVC.DTOs.Order
{
    public record class CreateOrderDTO
    {
    public List<CreateOrderDetailDTO> Products { get; init; } = new List<CreateOrderDetailDTO>();
    }
}