using gestionReservas.Data;
using gestionReservas.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionReservas.Services
{
    public class CanchaService
    {
        private readonly ApplicationDbContext _context;

        public CanchaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cancha>> ObtenerCanchasAsync()
        {
            return await _context.Canchas.ToListAsync();
        }
    }
}
