using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        [NotMapped]
        public decimal TotalPrice => OrderDetails.Sum(od => od.TotalPrice);
        public ICollection<OrderDetail> OrderDetails { get; set; }= new List<OrderDetail>();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}