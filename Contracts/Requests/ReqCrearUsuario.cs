using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqCrearUsuario
    {
        public string nombreUsuario { get; set; }

        public string nombreCompleto { get; set; }

        public string correo { get; set; }

        public string contrasena { get; set; }
    }
}
