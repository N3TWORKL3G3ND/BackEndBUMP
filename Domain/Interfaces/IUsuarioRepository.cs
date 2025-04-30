using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<(bool Success, string CodigoVerificacion, int? CodigoError, string DetalleError, string DetalleUsuario)> CrearUsuarioAsync(string nombreUsuario, string nombreCompleto, string correo, string contrasena);
        Task<(bool success, int? codigoError, string detalleError, string detalleUsuario)> ValidarCodigoVerificacionAsync(string correo, string codigoVerificacion);
    }
}
