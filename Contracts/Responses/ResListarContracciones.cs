using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResListarContracciones : ResBase
    {
        public List<ContraccionDto> listaContracciones { get; set; } = new List<ContraccionDto>();
    }
}
