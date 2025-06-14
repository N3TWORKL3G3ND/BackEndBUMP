using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarEventualidades : ResBase
    {
        public List<EventualidadDto> listaEventualidades { get; set; } = new List<EventualidadDto>();
    }
}
