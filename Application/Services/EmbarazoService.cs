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
    public class EmbarazoService : IEmbarazoService
    {
        private readonly IEmbarazoRepository _embarazoRepository;
        private readonly ISesionRepository _sesionRepository;

        public EmbarazoService(IEmbarazoRepository embarazoRepository, ISesionRepository sesionRepository)
        {
            _embarazoRepository = embarazoRepository;
            _sesionRepository = sesionRepository;
        }



        public async Task<ResBase> RegistrarEmbarazoAsync(ReqRegistrarEmbarazo request, ClaimsPrincipal user)
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
            if (request.fechaInicio == default)
                res.errores.Add("La fecha de inicio es obligatoria.");

            if (request.fechaEstimadaParto.HasValue && request.fechaEstimadaParto < request.fechaInicio)
                res.errores.Add("La fecha estimada de parto no puede ser anterior a la fecha de inicio.");

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
                res.detalle = "No se pudo registrar el embarazo porque no se identificó la sesión.";
                return res;
            }

            // 3. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario) = await _embarazoRepository.RegistrarEmbarazoAsync(
                    sessionGuid,
                    request.fechaInicio,
                    request.fechaEstimadaParto);

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 4. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Embarazo registrado exitosamente.";
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al registrar el embarazo.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al registrar el embarazo.";
                return res;
            }
        }



        public async Task<ResListarEmbarazos> ListarEmbarazosAsync(ClaimsPrincipal user)
        {
            var res = new ResListarEmbarazos
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaEmbarazos = new List<EmbarazoDto>()
            };

            // 1. Obtener el Session GUID del token
            Guid sessionGuid;
            var sessionGuidClaim = user.Claims.FirstOrDefault(c => c.Type == "session_guid")?.Value;

            if (string.IsNullOrWhiteSpace(sessionGuidClaim) || !Guid.TryParse(sessionGuidClaim, out sessionGuid))
            {
                res.errores.Add("No se pudo obtener la sesión del usuario actual.");
                res.detalle = "No se pudo listar los embarazos porque no se identificó la sesión.";
                return res;
            }

            // 2. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario, listaEmbarazos) = await _embarazoRepository.ListarEmbarazosAsync(sessionGuid);

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 3. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Lista de embarazos obtenida exitosamente.";
                res.listaEmbarazos = listaEmbarazos;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar los embarazos.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar los embarazos.";
                return res;
            }
        }



        public async Task<ResBase> RegistrarProgresoEmbarazoAsync(ReqRegistrarProgresoEmbarazo request)
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
            if (request.idEmbarazo <= 0)
                res.errores.Add("El ID de embarazo es obligatorio y debe ser mayor que cero.");

            if (request.pesoMadre.HasValue && request.pesoMadre <= 0)
                res.errores.Add("El peso de la madre debe ser mayor que cero.");

            if (request.tamanoBebe.HasValue && request.tamanoBebe <= 0)
                res.errores.Add("El tamaño del bebé debe ser mayor que cero.");

            if (res.errores.Count > 0)
            {
                res.detalle = "Errores de validación en los datos proporcionados.";
                return res;
            }

            // 2. Llamar al repositorio
            try
            {
                var (success, codigoError, detalleError, detalleUsuario) = await _embarazoRepository.RegistrarProgresoEmbarazoAsync(
                    request.idEmbarazo,
                    request.pesoMadre,
                    request.tamanoBebe,
                    request.notas
                );

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 3. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Progreso del embarazo registrado exitosamente.";
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al registrar el progreso del embarazo.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al registrar el progreso del embarazo.";
                return res;
            }
        }



        public async Task<ResListarProgresosEmbarazo> ListarProgresosEmbarazoAsync(int idEmbarazo)
        {
            var res = new ResListarProgresosEmbarazo
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaProgresos = new List<ProgresoEmbarazoDto>()
            };

            // 1. Validar que el ID del embarazo no sea nulo o inválido
            if (idEmbarazo <= 0)
            {
                res.errores.Add("El ID del embarazo es inválido.");
                res.detalle = "No se pudo listar los progresos del embarazo debido a un ID inválido.";
                return res;
            }

            // 2. Llamar al repositorio para listar los progresos de embarazo
            try
            {
                var (success, codigoError, detalleError, detalleUsuario, listaProgresos) = await _embarazoRepository.ListarProgresosEmbarazoAsync(idEmbarazo);

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 3. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Lista de progresos de embarazo obtenida exitosamente.";
                res.listaProgresos = listaProgresos;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar los progresos del embarazo.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar los progresos del embarazo.";
                return res;
            }
        }



        public async Task<ResListarHospitales> ListarHospitalesAsync()
        {
            var res = new ResListarHospitales
            {
                resultado = false,
                detalle = string.Empty,
                errores = new List<string>(),
                listaHospitales = new List<HospitalDto>()
            };

            // 1. Llamar al repositorio para listar los hospitales
            try
            {
                var (success, codigoError, detalleError, detalleUsuario, listaHospitales) = await _embarazoRepository.ListarHospitalesAsync();

                if (!success)
                {
                    res.errores.Add(ErrorCodigoExtensions.GetDescription(ErrorCodigoExtensions.ObtenerCodigoErrorEnum(codigoError)));
                    res.detalle = detalleUsuario;
                    return res;
                }

                // 2. Respuesta exitosa
                res.resultado = true;
                res.detalle = "Lista de hospitales obtenida exitosamente.";
                res.listaHospitales = listaHospitales;
                return res;
            }
            catch (SqlException ex)
            {
                res.errores.Add($"Error en la base de datos: {ex.Message}");
                res.detalle = "Ocurrió un error al listar los hospitales.";
                return res;
            }
            catch (Exception ex)
            {
                res.errores.Add($"Error inesperado: {ex.Message}");
                res.detalle = "Ocurrió un error inesperado al listar los hospitales.";
                return res;
            }
        }




























    }
}
