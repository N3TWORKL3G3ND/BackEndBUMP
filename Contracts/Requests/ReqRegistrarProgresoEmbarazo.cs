using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRegistrarProgresoEmbarazo
    {
        public int idEmbarazo { get; set; }                // ID obligatorio del embarazo
        public decimal? pesoMadre { get; set; }            // Peso opcional de la madre
        public decimal? tamanoBebe { get; set; }           // Tamaño opcional del bebé
        public string? notas { get; set; }                 // Notas opcionales
    }
}
