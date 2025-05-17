using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResDesplegarCita : ResBase
    {
        public DesplegarCitaDto? cita { get; set; }
    }
}
