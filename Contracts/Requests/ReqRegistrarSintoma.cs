using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRegistrarSintoma
    {
        public int idSintoma { get; set; }
        public string? notas { get; set; }
    }
}
