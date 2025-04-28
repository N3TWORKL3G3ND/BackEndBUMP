using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;

        public UsuarioService(IUsuarioRepository usuarioRepository, IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
        }


        public async Task<ResBase> CrearUsuarioAsync(ReqCrearUsuario request)
        {
            var res = new ResBase
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>()
            };

            // 0. Validar que el request no sea nulo
            if (request == null)
            {
                res.errores.Add("El request no puede ser nulo.");
                res.detalle = "El request no puede ser nulo.";
                return res;
            }

            // 1. Validar datos de entrada
            if (string.IsNullOrWhiteSpace(request.nombreUsuario))
                res.errores.Add("El nombre de usuario es obligatorio.");
            else if (request.nombreUsuario.Length > 50)
                res.errores.Add("El nombre de usuario no puede exceder los 50 caracteres.");

            if (string.IsNullOrWhiteSpace(request.nombreCompleto))
                res.errores.Add("El nombre completo es obligatorio.");
            else if (request.nombreCompleto.Length > 100)
                res.errores.Add("El nombre completo no puede exceder los 100 caracteres.");

            if (string.IsNullOrWhiteSpace(request.correo))
                res.errores.Add("El correo es obligatorio.");
            else if (request.correo.Length > 100)
                res.errores.Add("El correo no puede exceder los 100 caracteres.");
            else if (!Regex.IsMatch(request.correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                res.errores.Add("El correo debe ser válido.");

            if (string.IsNullOrWhiteSpace(request.contrasena))
                res.errores.Add("La contraseña es obligatoria.");
            else if (request.contrasena.Length > 100)
                res.errores.Add("La contraseña no puede exceder los 100 caracteres.");

            if (res.errores.Count > 0)
            {
                res.detalle = "Errores de validación en los datos proporcionados.";
                return res;
            }

            // 2. Llamar al repositorio
            try
            {
                var (success, codigoVerificacion, codigoError, detalleError, detalleUsuario) = await _usuarioRepository.CrearUsuarioAsync(
                    request.nombreUsuario,
                    request.nombreCompleto,
                    request.correo,
                    request.contrasena);

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 3. Enviar correo con el código de verificación
                try
                {
                    await _emailService.SendAsync(
                        request.correo,
                        "Verificación de Cuenta - BUMP",
                        $"Tu código de verificación es: {codigoVerificacion}. Este código expira en 24 horas.");
                }
                catch (Exception ex)
                {
                    res.errores.Add($"Error al enviar el correo de verificación: {ex.Message}");
                    res.detalle = "Usuario creado, pero no se pudo enviar el correo de verificación.";
                    return res;
                }

                // 4. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Usuario creado exitosamente. Se ha enviado un correo con el código de verificación.";
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al crear el usuario.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al crear el usuario.";
                return res;
            }
        }













    }
}
