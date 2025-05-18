using Data.Contexts;
using Domain.Interfaces;

namespace Data.Repositories
{
    public class SeguimientoRepository : ISeguimientoRepository 
    {
        private readonly BumpContext _context;

        public SeguimientoRepository(BumpContext context)
        {
            _context = context;
        }









    }
}
