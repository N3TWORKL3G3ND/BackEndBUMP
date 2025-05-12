using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarEmbarazos : ResBase
    {
        public List<EmbarazoDto> listaEmbarazos { get; set; } = new List<EmbarazoDto>();
    }
}
