using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISeguimientoRepository
    {
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<SintomaDto> ListaSintomas)> ListarSintomasCatalogoAsync();
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarSintomaAsync(Guid sessionGuid, int idSintoma, string? notas);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarEventualidadAsync(Guid sessionGuid, DateTime fechaEventualidad, TimeSpan horaEventualidad, string descripcion, byte gravedad);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario)> RegistrarContraccionAsync(Guid sessionGuid, DateTime fechaInicio, TimeSpan horaInicio, int duracionSegundos, byte intensidad);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<ContraccionDto> ListaContracciones)> ListarContraccionesAsync(Guid sessionGuid);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<EventualidadDto> ListaEventualidades)> ListarEventualidadesAsync(Guid sessionGuid);
        Task<(bool Success, int? CodigoError, string DetalleError, string DetalleUsuario, List<RegistroSintomaDto> ListaRegistroSintomas)> ListarRegistroSintomasAsync(Guid sessionGuid);
    }
}
