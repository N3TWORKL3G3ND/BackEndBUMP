using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqIniciarSesion
    {
        public string correo { get; set; }
        public string contrasena { get; set; }

    }
}
