using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using App.MVC.Entities;
using App.MVC.Helpers.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace App.MVC.Helpers
{
    public class Jwt: IJwt
    {
        ILogger<Jwt> _logger;
        private readonly JwtSettings _jwtSettings;
        public Jwt(ILogger<Jwt> logger, JwtSettings jwtSettings)
        {
            _logger = logger;
            _jwtSettings = jwtSettings;

        }
        public string GenerateJwtToken(string email, string role)
        {
            try
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                        issuer: _jwtSettings.Issuer,
                        audience: _jwtSettings.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                        signingCredentials: creds
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token for user with email {Email}", email);
                throw;
            }
        }
    }
}