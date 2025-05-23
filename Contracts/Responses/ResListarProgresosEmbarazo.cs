using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarProgresosEmbarazo : ResBase
    {
        public List<ProgresoEmbarazoDto> listaProgresos { get; set; } = new List<ProgresoEmbarazoDto>();
    }
}
