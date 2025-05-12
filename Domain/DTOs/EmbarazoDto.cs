using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EmbarazoDto
    {
        public int IdEmbarazo { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaEstimadaParto { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
