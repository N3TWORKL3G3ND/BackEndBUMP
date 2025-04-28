using Contracts.Requests;
using Contracts.Responses;

namespace Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResBase> CrearUsuarioAsync(ReqCrearUsuario request);
    }
}
