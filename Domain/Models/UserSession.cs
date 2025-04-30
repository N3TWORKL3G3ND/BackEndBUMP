using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class UserSession
    {
        public Guid sessionGuid { get; set; }
        public int idUsuario { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime gechaExpiracion { get; set; }
        public bool activa { get; set; }
    }
}
