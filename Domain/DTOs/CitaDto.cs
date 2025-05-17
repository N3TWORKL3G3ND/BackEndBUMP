using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CitaDto
    {
        public int IdCita { get; set; }
        public DateTime FechaHoraCita { get; set; }
        public byte Estado { get; set; }
    }
}
