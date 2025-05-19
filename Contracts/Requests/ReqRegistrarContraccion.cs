using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRegistrarContraccion
    {
        public DateTime fechaInicio { get; set; }
        public TimeSpan horaInicio { get; set; }
        public int duracionSegundos { get; set; }
        public byte intensidad { get; set; }
    }
}
