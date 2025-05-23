using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ProgresoEmbarazoDto
    {
        public int IdProgreso { get; set; }
        public int IdEmbarazo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int SemanaEmbarazo { get; set; }
        public decimal? PesoMadre { get; set; }
        public decimal? TamanoBebe { get; set; }
        public string? Notas { get; set; }
        public string? Accion { get; set; }
        public int? UsuarioAccion { get; set; }
        public string? DatoAnterior { get; set; }
        public DateTime? FechaAccion { get; set; }
    }
}
