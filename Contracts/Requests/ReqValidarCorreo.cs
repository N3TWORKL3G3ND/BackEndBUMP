using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqValidarCorreo
    {
        public string correo { get; set; }
        
        public string codigoVerificacion { get; set; }
    }
}
