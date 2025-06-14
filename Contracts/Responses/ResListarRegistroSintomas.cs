using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarRegistroSintomas : ResBase
    {
        public List<RegistroSintomaDto> listaRegistroSintomas { get; set; } = new List<RegistroSintomaDto>();
    }
}
