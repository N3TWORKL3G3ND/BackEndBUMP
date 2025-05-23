using Contracts.Requests;
using Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISesionService
    {
        Task<ResLoginUsuario> LoginUsuarioAsync(ReqLoginUsuario request);
        Task<ResBase> CerrarSesionAsync();
    }
}
