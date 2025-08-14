using System;
using System.Threading.Tasks;
using App.MVC.DTOs.Auth;
using App.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace App.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginRequestDTO);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return Unauthorized(new { Error = result.ErrorMessage });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.RegisterAsync(registerRequestDTO);
            if (result.Success)
            {
                return CreatedAtAction(nameof(Register), new { Email = registerRequestDTO.Email }, new { Message = result.Data });
            }

            return BadRequest(new { Error = result.ErrorMessage });
        }
    }
}
