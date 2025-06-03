using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRestablecerContrasena
    {
        public string Correo { get; set; }
        public string CodigoVerificacion { get; set; }
        public string NuevaContrasena { get; set; }
    }
}
