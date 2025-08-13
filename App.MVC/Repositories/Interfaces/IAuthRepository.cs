using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Entities;

namespace App.MVC.Repositories.Interfaces
{
    public interface IAuthRepository :IGenericRepository<User>
    {
        Task<User?> GetUserByEmail(string email);
        Task<bool> IsExist(string email);
    }
}