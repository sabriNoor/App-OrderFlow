using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs.Auth;
using App.MVC.Entities;

namespace App.MVC.Mappers
{
    public static class UserMapper
    {
        public static User ToModel(this RegisterRequestDTO registerDTO)
        {
            return new User
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                Email = registerDTO.Email,
            };
        }
    }
}