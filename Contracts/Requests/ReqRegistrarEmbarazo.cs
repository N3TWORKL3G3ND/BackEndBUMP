using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRegistrarEmbarazo
    {
        public DateTime fechaInicio { get; set; }
        public DateTime? fechaEstimadaParto { get; set; }
    }
}
