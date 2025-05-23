using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarConsejos : ResBase
    {
        public List<ConsejoDto> listaConsejos { get; set; } = new List<ConsejoDto>();
    }
}
