using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RegistroSintomaDto
    {
        public int IdRegistroSintoma { get; set; }
        public int IdEmbarazo { get; set; }
        public int IdSintoma { get; set; }
        public string NombreSintoma { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Notas { get; set; }
    }
}
