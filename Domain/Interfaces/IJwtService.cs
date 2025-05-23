using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(string nombreUsuario, string correo, Guid sessionGuid);
        Task<bool> ValidateSessionAsync(string sessionGuid);
    }
}
