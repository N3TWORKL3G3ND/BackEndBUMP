using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Requests
{
    public class ReqRegistrarCita
    {
        public int idHospital { get; set; }           // ID del hospital donde se realizará la cita
        public DateTime fechaHoraCita { get; set; }  // Fecha y hora de la cita (puede ser nula)
        public string descripcion { get; set; }       // Descripción opcional de la cita
        public byte estado { get; set; }              // Estado de la cita (0: Pendiente, 1: Confirmada, 2: Cancelada)
    }
}
