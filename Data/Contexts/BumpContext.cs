using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public class BumpContext : DbContext
    {
        public BumpContext(DbContextOptions<BumpContext> options) : base(options) { }
    }
}
