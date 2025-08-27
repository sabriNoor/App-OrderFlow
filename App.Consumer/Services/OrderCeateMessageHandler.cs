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
            if (order is null) return "";

            var htmlBody = new StringBuilder();

            htmlBody.AppendLine(HtmlHeader());
            htmlBody.AppendLine("<body>");
            htmlBody.AppendLine("<div class='order-container'>");
            htmlBody.AppendLine(OrderHeader(order.Id, order.TotalPrice));
            htmlBody.AppendLine(OrderTable(order.OrderDetails));
            htmlBody.AppendLine(OrderFooter());
            htmlBody.AppendLine("</div>");
            htmlBody.AppendLine("</body>");
            htmlBody.AppendLine("</html>");

            return htmlBody.ToString();
        }

        private string HtmlHeader()
        {
            return @"
                    <!DOCTYPE html>
                    <html>
                    <head>
                    <style>
                    body { font-family: Arial, sans-serif; color: #333; }
                    .order-container { max-width: 600px; margin: auto; padding: 20px; border: 1px solid #eee; border-radius: 8px; }
                    h3 { color: #2a9d8f; }
                    table { width: 100%; border-collapse: collapse; margin-top: 10px; }
                    th, td { padding: 8px 12px; border: 1px solid #ddd; text-align: left; }
                    th { background-color: #f4f4f4; }
                    .total { font-weight: bold; }
                    .footer { margin-top: 20px; font-size: 0.9em; color: #555; }
                    </style>
                    </head>";
        }

        private string OrderHeader(int orderId, decimal totalPrice)
        {
            return $"<h3>Order Confirmation - ID: {orderId}</h3><p>Total Price: <strong>{totalPrice:C}</strong></p>";
        }

        private string OrderTable(IEnumerable<OrderDetailsDTO> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table>");
            sb.AppendLine("<thead><tr><th>Product</th><th>Quantity</th><th>Price</th></tr></thead>");
            sb.AppendLine("<tbody>");
            foreach (var item in items)
            {
                sb.AppendLine($"<tr><td>{item.ProductName}</td><td>{item.Quantity}</td><td>{item.TotalPrice:C}</td></tr>");
            }
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            return sb.ToString();
        }

        private string OrderFooter()
        {
            return "<p class='footer'>Thank you for your order! We appreciate your business and hope to serve you again soon.</p>";
        }

    }
}