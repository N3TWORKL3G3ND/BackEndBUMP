using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class JwtSettings
    {
        public string secretKey { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
        public int expiryMinutes { get; set; }
    }
}
