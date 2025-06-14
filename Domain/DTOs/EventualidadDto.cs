using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class EventualidadDto
    {
        public int IdEventualidad { get; set; }
        public int IdEmbarazo { get; set; }
        public DateTime FechaEventualidad { get; set; }
        public TimeSpan HoraEventualidad { get; set; }
        public string Descripcion { get; set; }
        public byte Gravedad { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
