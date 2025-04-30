using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISesionRepository
    {
        Task<(bool Success, string NombreUsuario, bool CorreoVerificado, Guid SessionGuid, int? CodigoError, string DetalleError, string DetalleUsuario)> LoginUsuarioAsync(string correo, string contrasena);
        Task<(bool success, string detalleError)> ValidarSesionAsync(Guid sessionGuid);
        Task<(bool success, string detalleError, string detalleUsuario)> CerrarSesionAsync(Guid sessionGuid);
    }
}
