using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.Data;
using App.MVC.Entities;
using App.MVC.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.MVC.Repositories
{
    public class AuthRepository : GenericRepository<User>, IAuthRepository
    {

        public AuthRepository(ApplicationDBContext dBContext) : base(dBContext)
        {
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
        
        public async Task<bool> IsExist(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }
    }
}