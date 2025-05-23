using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ConsejoDto
    {
        public int IdConsejo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int SemanaEmbarazo { get; set; }
    }
}
