using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SesionService : ISesionService
    {
        private readonly ISesionRepository _sesionRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SesionService(ISesionRepository sesionRepository, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _sesionRepository = sesionRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<ResLoginUsuario> LoginUsuarioAsync(ReqLoginUsuario request)
        {
            var res = new ResLoginUsuario
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                correoVerificado = false
            };

            if (request == null)
            {
                res.errores.Add("El request no puede ser nulo.");
                res.detalle = "El request no puede ser nulo.";
                return res;
            }

            if (string.IsNullOrWhiteSpace(request.correo))
                res.errores.Add("El correo es obligatorio.");
            else if (!Regex.IsMatch(request.correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                res.errores.Add("El correo debe ser válido.");

            if (string.IsNullOrWhiteSpace(request.contrasena))
                res.errores.Add("La contraseña es obligatoria.");

            if (res.errores.Count > 0)
            {
                res.detalle = "Errores de validación en los datos proporcionados.";
                return res;
            }

            try
            {
                var (success, nombreUsuario, correoVerificado, sessionGuid, codigoError, detalleError, detalleUsuario) = await _sesionRepository.LoginUsuarioAsync(
                    request.correo,
                    request.contrasena);

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                var token = _jwtService.GenerateJwtToken(nombreUsuario, request.correo, sessionGuid);
                res.resultado = true;
                res.detalle = "Inicio de sesión exitoso.";
                res.token = token;
                res.correoVerificado = correoVerificado;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al iniciar sesión.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al iniciar sesión.";
                return res;
            }
        }



        public async Task<ResBase> CerrarSesionAsync()
        {
            var res = new ResBase
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>()
            };

            var sessionGuidClaim = _httpContextAccessor.HttpContext?.User.FindFirst("session_guid")?.Value;
            if (string.IsNullOrEmpty(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out var sessionGuid))
            {
                res.errores.Add("No se encontró una sesión válida.");
                res.detalle = "No se pudo cerrar la sesión.";
                return res;
            }

            try
            {
                var (success, detalleError, detalleUsuario) = await _sesionRepository.CerrarSesionAsync(sessionGuid);
                if (!success)
                {
                    res.errores.Add(detalleUsuario);
                    res.detalle = detalleUsuario;
                    return res;
                }

                res.resultado = true;
                res.detalle = detalleUsuario;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al cerrar la sesión.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al cerrar la sesión.";
                return res;
            }
        }




































    }
}

