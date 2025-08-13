using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs;
using App.MVC.DTOs.Auth;

namespace App.MVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> Login(LoginRequestDTO loginRequestDTO);
        Task<ServiceResult<string>> Register(RegisterRequestDTO registerRequestDTO);
    }
}