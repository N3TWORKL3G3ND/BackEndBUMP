using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DesplegarCitaDto
    {
        public int IdCita { get; set; }
        public DateTime FechaHoraCita { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public HospitalDto Hospital { get; set; } = new HospitalDto();
    }
}
