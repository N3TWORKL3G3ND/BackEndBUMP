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
    public interface IEmbarazoService
    {
        Task<ResBase> RegistrarEmbarazoAsync(ReqRegistrarEmbarazo request, ClaimsPrincipal user);
        Task<ResListarEmbarazos> ListarEmbarazosAsync(ClaimsPrincipal user);
        Task<ResBase> RegistrarProgresoEmbarazoAsync(ReqRegistrarProgresoEmbarazo request);
        Task<ResListarProgresosEmbarazo> ListarProgresosEmbarazoAsync(int idEmbarazo);
        Task<ResListarHospitales> ListarHospitalesAsync();
    }
}
