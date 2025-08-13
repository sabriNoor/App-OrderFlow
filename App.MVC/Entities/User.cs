using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Constants;

namespace App.MVC.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; } = [];
        [Required]
        public byte[] PasswordSalt { get; set; } = [];

        [StringLength(50)]
        public string Role { get; set; } = Roles.User;

        public ICollection<Order> Orders = [];
    }
}