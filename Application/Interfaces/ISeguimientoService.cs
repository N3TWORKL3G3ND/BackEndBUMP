using Contracts.Requests;
using Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISeguimientoService
    {
        Task<ResListarSintomas> ListarSintomasCatalogoAsync();
        Task<ResBase> RegistrarSintomaAsync(ReqRegistrarSintoma request, ClaimsPrincipal user);
        Task<ResBase> RegistrarEventualidadAsync(ReqRegistrarEventualidad request, ClaimsPrincipal user);
        Task<ResBase> RegistrarContraccionAsync(ReqRegistrarContraccion request, ClaimsPrincipal user);
    }
}
