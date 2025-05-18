using Application.Interfaces;
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

    }
}
