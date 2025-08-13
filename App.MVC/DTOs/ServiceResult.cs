using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.MVC.DTOs
{
    public record ServiceResult<T>(bool Success, T? Data = default, string? ErrorMessage = null)
    {
        public static ServiceResult<T> Ok(T data) => new(true, data);
        public static ServiceResult<T> Fail(string errorMessage) => new(false, default, errorMessage);
    }
}