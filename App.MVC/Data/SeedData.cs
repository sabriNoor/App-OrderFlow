using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Constants;
using App.MVC.Entities;
using App.MVC.Helpers;

namespace App.MVC.Data
{
    public static class SeedData
{
    public static void SeedAdmin(ApplicationDBContext context)
    {
            if (!context.Users.Any(u => u.Role == Roles.Admin))
            {
                PasswordHasher.CreatePasswordHash("Admin@123456", out byte[] passwordHash, out byte[] passwordSalt);

                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "Super",
                    Email = "admin@gmail.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = Roles.Admin
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
            
    }
}

}