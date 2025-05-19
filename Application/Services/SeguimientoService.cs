using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Domain.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SeguimientoService : ISeguimientoService
    {
        private readonly ISeguimientoRepository _seguimientoRepository;
        private readonly ISesionRepository _sesionRepository;

        public SeguimientoService(ISeguimientoRepository seguimientoRepository, ISesionRepository sesionRepository)
        {
            _seguimientoRepository = seguimientoRepository;
            _sesionRepository = sesionRepository;
        }


        public async Task<ResListarSintomas> ListarSintomasCatalogoAsync()
        {
            var res = new ResListarSintomas
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaSintomas = new List<SintomaDto>()
            };

            // 2. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario, listaSintomas) = await _seguimientoRepository.ListarSintomasCatalogoAsync();

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 3. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Lista de síntomas obtenida exitosamente.";
                res.listaSintomas = listaSintomas;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar los síntomas.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar los síntomas.";
                return res;
            }
        }



        public async Task<ResBase> RegistrarSintomaAsync(ReqRegistrarSintoma request, ClaimsPrincipal user)
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
            if (request.idSintoma <= 0)
                res.errores.Add("El ID del síntoma es obligatorio y debe ser mayor que cero.");

            if (res.errores.Count > 0)
            {
                res.detalle = "Errores de validación en los datos proporcionados.";
                return res;
            }

            // 2. Obtener el Session GUID del token
            Guid sessionGuid;
            var sessionGuidClaim = user.Claims.FirstOrDefault(c => c.Type == "session_guid")?.Value;

            if (string.IsNullOrWhiteSpace(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out sessionGuid))
            {
                res.errores.Add("No se pudo obtener la sesión del usuario actual.");
                res.detalle = "No se pudo registrar el síntoma porque no se identificó la sesión.";
                return res;
            }

            // 3. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario) = await _seguimientoRepository.RegistrarSintomaAsync(
                    sessionGuid,
                    request.idSintoma,
                    request.notas
                );

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 4. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Síntoma registrado exitosamente.";
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al registrar el síntoma.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al registrar el síntoma.";
                return res;
            }
        }



        public async Task<ResBase> RegistrarEventualidadAsync(ReqRegistrarEventualidad request, ClaimsPrincipal user)
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
            if (request.fechaEventualidad == null)
                res.errores.Add("La fecha de la eventualidad es obligatoria.");

            if (request.horaEventualidad == null)
                res.errores.Add("La hora de la eventualidad es obligatoria.");

            if (string.IsNullOrWhiteSpace(request.descripcion))
                res.errores.Add("La descripción de la eventualidad es obligatoria.");

            if (request.gravedad < 1 || request.gravedad > 3)
                res.errores.Add("La gravedad debe ser 1 (Leve), 2 (Moderada) o 3 (Grave).");

            if (res.errores.Count > 0)
            {
                res.detalle = "Errores de validación en los datos proporcionados.";
                return res;
            }

            // 2. Obtener el Session GUID del token
            Guid sessionGuid;
            var sessionGuidClaim = user.Claims.FirstOrDefault(c => c.Type == "session_guid")?.Value;

            if (string.IsNullOrWhiteSpace(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out sessionGuid))
            {
                res.errores.Add("No se pudo obtener la sesión del usuario actual.");
                res.detalle = "No se pudo registrar la eventualidad porque no se identificó la sesión.";
                return res;
            }

            // 3. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario) = await _seguimientoRepository.RegistrarEventualidadAsync(
                    sessionGuid,
                    request.fechaEventualidad,
                    request.horaEventualidad,
                    request.descripcion,
                    (byte)request.gravedad
                );

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 4. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Eventualidad registrada exitosamente.";
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al registrar la eventualidad.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al registrar la eventualidad.";
                return res;
            }
        }



























    }
}
