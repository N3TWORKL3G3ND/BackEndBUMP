using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRegistrarEventualidad
    {
        public DateTime fechaEventualidad { get; set; }
        public TimeSpan horaEventualidad { get; set; }
        public string descripcion { get; set; }
        public int gravedad { get; set; }
    }
}
