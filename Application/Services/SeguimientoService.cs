﻿using Application.Interfaces;
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



        public async Task<ResBase> RegistrarContraccionAsync(ReqRegistrarContraccion request, ClaimsPrincipal user)
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
            if (request.fechaInicio == null)
                res.errores.Add("La fecha de inicio es obligatoria.");

            if (request.horaInicio == null)
                res.errores.Add("La hora de inicio es obligatoria.");

            if (request.duracionSegundos <= 0)
                res.errores.Add("La duración debe ser mayor a cero segundos.");

            if (request.intensidad < 1 || request.intensidad > 3)
                res.errores.Add("La intensidad debe ser 1 (Leve), 2 (Moderada) o 3 (Fuerte).");

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
                res.detalle = "No se pudo registrar la contracción porque no se identificó la sesión.";
                return res;
            }

            // 3. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario) = await _seguimientoRepository.RegistrarContraccionAsync(
                    sessionGuid,
                    request.fechaInicio,
                    request.horaInicio,
                    request.duracionSegundos,
                    (byte)request.intensidad
                );

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 4. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Contracción registrada exitosamente.";
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al registrar la contracción.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al registrar la contracción.";
                return res;
            }
        }



        public async Task<ResListarContracciones> ListarContraccionesAsync(ClaimsPrincipal user)
        {
            var res = new ResListarContracciones
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaContracciones = new List<ContraccionDto>()
            };

            // 2. Obtener el Session GUID del token
            Guid sessionGuid;
            var sessionGuidClaim = user.Claims.FirstOrDefault(c => c.Type == "session_guid")?.Value;

            if (string.IsNullOrWhiteSpace(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out sessionGuid))
            {
                res.errores.Add("No se pudo obtener la sesión del usuario actual.");
                res.detalle = "No se pudo registrar la eventualidad porque no se identificó la sesión.";
                return res;
            }

            try
            {
                // Llamar al repositorio
                var (success, codigoError, detalleError, detalleUsuario, listaContracciones) =
                    await _seguimientoRepository.ListarContraccionesAsync(sessionGuid);

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(
                        ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));

                    res.detalle = detalleUsuario;
                    return res;
                }

                // Respuesta exitosa
                res.resultado = true;
                res.detalle = "Lista de contracciones obtenida exitosamente.";
                res.listaContracciones = listaContracciones;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar las contracciones.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar las contracciones.";
                return res;
            }
        }



        public async Task<ResListarEventualidades> ListarEventualidadesAsync(ClaimsPrincipal user)
        {
            var res = new ResListarEventualidades
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaEventualidades = new List<EventualidadDto>()
            };

            // 2. Obtener el Session GUID del token
            Guid sessionGuid;
            var sessionGuidClaim = user.Claims.FirstOrDefault(c => c.Type == "session_guid")?.Value;

            if (string.IsNullOrWhiteSpace(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out sessionGuid))
            {
                res.errores.Add("No se pudo obtener la sesión del usuario actual.");
                res.detalle = "No se pudo registrar la eventualidad porque no se identificó la sesión.";
                return res;
            }

            try
            {
                // Llamar al repositorio
                var (success, codigoError, detalleError, detalleUsuario, listaEventualidades) =
                    await _seguimientoRepository.ListarEventualidadesAsync(sessionGuid);

                if (!success)
                {
                    var codigoEnum = ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError ?? -1);
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(codigoEnum));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // Respuesta exitosa
                res.resultado = true;
                res.detalle = "Lista de eventualidades obtenida exitosamente.";
                res.listaEventualidades = listaEventualidades;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar las eventualidades.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar las eventualidades.";
                return res;
            }
        }



        public async Task<ResListarRegistroSintomas> ListarRegistroSintomasAsync(ClaimsPrincipal user)
        {
            var res = new ResListarRegistroSintomas
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaRegistroSintomas = new List<RegistroSintomaDto>()
            };

            // 2. Obtener el Session GUID del token
            Guid sessionGuid;
            var sessionGuidClaim = user.Claims.FirstOrDefault(c => c.Type == "session_guid")?.Value;

            if (string.IsNullOrWhiteSpace(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out sessionGuid))
            {
                res.errores.Add("No se pudo obtener la sesión del usuario actual.");
                res.detalle = "No se pudo registrar la eventualidad porque no se identificó la sesión.";
                return res;
            }

            try
            {
                var (success, codigoError, detalleError, detalleUsuario, listaRegistroSintomas) =
                    await _seguimientoRepository.ListarRegistroSintomasAsync(sessionGuid);

                if (!success)
                {
                    var codigoEnum = ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError ?? -1);
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(codigoEnum));
                    res.detalle = detalleUsuario;
                    return res;
                }

                res.resultado = true;
                res.detalle = "Lista de síntomas registrados obtenida exitosamente.";
                res.listaRegistroSintomas = listaRegistroSintomas;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar los síntomas registrados.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar los síntomas registrados.";
                return res;
            }
        }





















    }
}
