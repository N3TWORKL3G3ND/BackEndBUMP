using Data.Contexts;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data.InternalServices
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly ISesionRepository _sesionRepository;

        public JwtService(IOptions<JwtSettings> jwtSettings, ISesionRepository sesionRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _sesionRepository = sesionRepository;
        }



        public string GenerateJwtToken(string nombreUsuario, string correo, Guid sessionGuid)
        {
            var claims = new[]
            {
            new Claim("session_guid", sessionGuid.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, nombreUsuario),
            new Claim(JwtRegisteredClaimNames.Email, correo),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.issuer,
                audience: _jwtSettings.audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.expiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<bool> ValidateSessionAsync(string sessionGuid)
        {
            if (!Guid.TryParse(sessionGuid, out var guid))
                return false;

            var (success, detalleError) = await _sesionRepository.ValidarSesionAsync(guid);
            return success;
        }




































    }
}
