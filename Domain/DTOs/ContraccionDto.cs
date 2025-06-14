using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ContraccionDto
    {
        public int IdContraccion { get; set; }
        public int IdEmbarazo { get; set; }
        public DateTime FechaInicio { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public int DuracionSegundos { get; set; }
        public byte Intensidad { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
