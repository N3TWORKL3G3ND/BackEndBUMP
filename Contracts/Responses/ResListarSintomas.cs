using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarSintomas : ResBase
    {
        public List<SintomaDto> listaSintomas { get; set; } = new List<SintomaDto>();
    }
}
