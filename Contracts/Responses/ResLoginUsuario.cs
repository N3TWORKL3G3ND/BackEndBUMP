using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Responses
{
    public class ResLoginUsuario : ResBase
    {
        public string token { get; set; }

        public bool correoVerificado { get; set; }

    }
}
