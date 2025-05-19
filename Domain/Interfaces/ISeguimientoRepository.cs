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
    }
}
