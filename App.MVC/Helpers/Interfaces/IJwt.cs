using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.Helpers.Interfaces
{
    public interface IJwt
    {
        string GenerateJwtToken(string email, string role,int id);
    }
}