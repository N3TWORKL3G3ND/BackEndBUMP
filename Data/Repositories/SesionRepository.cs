using Data.Contexts;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Repositories
{
    public class SesionRepository : ISesionRepository
    {
        private readonly BumpContext _context;

        public SesionRepository(BumpContext context)
        {
            _context = context;
        }

        
































    }
}
