using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Consumer.DTOs;
using App.Consumer.Services.Interfaces;
using Newtonsoft.Json;

namespace App.Consumer.Services
{
    public class OrderCeateMessageHandler : IEmailBodyBuilder
    {


        public string GenerateEmailBody(string body)
        {
            var order = JsonConvert.DeserializeObject<OrderCreateDTO>(body);
            if (order is null)
            {
                return "";
            }
            var htmlBody = new StringBuilder();
            htmlBody.AppendLine($"<h3>Order ID: {order.Id}</h3>");
            htmlBody.AppendLine($"<p>Total Price: {order.TotalPrice:C}</p>");
            htmlBody.AppendLine("<ul>");
            foreach (var item in order.OrderDetails)
            {
                htmlBody.AppendLine($"<li>{item.ProductName} x{item.Quantity} = {item.TotalPrice:C}</li>");
            }
            htmlBody.AppendLine("</ul>");
            htmlBody.AppendLine($"<p> Thank you for your order. </p>");

            return htmlBody.ToString();

        }
    }
}