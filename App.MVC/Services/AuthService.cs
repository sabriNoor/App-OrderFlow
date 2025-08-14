using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MVC.DTOs;
using App.MVC.DTOs.Auth;
using App.MVC.Helpers;
using App.MVC.Helpers.Interfaces;
using App.MVC.Mappers;
using App.MVC.Repositories.Interfaces;
using App.MVC.Services.Interfaces;

namespace App.MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwt _jwt;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IAuthRepository authRepository, IJwt jwt, ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _jwt = jwt;
            _logger = logger;
        }

        public async Task<ServiceResult<string>> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var user = await _authRepository.GetUserByEmail(loginRequestDTO.Email);
                if (user is null)
                {
                    _logger.LogWarning("Login failed: user with email {Eamil} not found", loginRequestDTO.Email);
                    return ServiceResult<string>.Fail("User not found.");
                }
                if (!PasswordHasher.VerifyPasswordHash(loginRequestDTO.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _logger.LogWarning("Login failed: user with email {Email} entered incorrect password", user.Email);
                    return ServiceResult<string>.Fail("Invalid password.");
                }
                var token = _jwt.GenerateJwtToken(user.Email, user.Role,user.Id);
                _logger.LogInformation("Login success: user {FirstName} {LastName} with email {Email} and role {Role} logged in successfully",
                     user.FirstName, user.LastName, user.Email, user.Role);
                return ServiceResult<string>.Ok(token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during login for email {Email}", loginRequestDTO.Email);
                return ServiceResult<string>.Fail("An unexpected error occurred. Please try again later.");
            }



        }

        public async Task<ServiceResult<string>> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {

            try
            {
                var user = registerRequestDTO.ToModel();
                var alreadyExist = await _authRepository.IsExist(user.Email);
                if (alreadyExist)
                {
                    _logger.LogWarning("Signup failed: Email {Email} already exists", user.Email);
                    return ServiceResult<string>.Fail("Email is already in use.");
                }
                PasswordHasher.CreatePasswordHash(registerRequestDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                await _authRepository.AddAsync(user);
                _logger.LogInformation("Signup success: a new user {FirstName} {LastName} with email {Email} and role {Role} registerd successfully",
                    user.FirstName, user.LastName, user.Email, user.Role);

                return ServiceResult<string>.Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while registering email {Email}", registerRequestDTO.Email);
                return ServiceResult<string>.Fail("An unexpected error occurred. Please try again later.");
            }


        }

    }
}