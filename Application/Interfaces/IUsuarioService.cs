using Contracts.Requests;
using Contracts.Responses;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResBase> CrearUsuarioAsync(ReqCrearUsuario request);
        Task<ResBase> ValidarCorreoAsync(ReqValidarCorreo request);
        Task<ResBase> GenerarCodigoVerificacionAsync(ReqGenerarCodigo req);
        Task<ResBase> RestablecerContrasenaAsync(ReqRestablecerContrasena req);
        Task<ResDatosUsuario> ObtenerDatosUsuarioAsync(ClaimsPrincipal user);
    }
}
