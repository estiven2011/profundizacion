using gestionReservas.Data;
using gestionReservas.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionReservas.Services
{
    public class ReservaService
    {
        private readonly ApplicationDbContext _context;

        public ReservaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reserva>> ObtenerReservasAsync()
        {
            return await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Usuario)
                .ToListAsync();
        }

        public async Task CrearReservaAsync(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task EditarReservaAsync(Reserva reservaActualizada)
        {
            var reservaExistente = await _context.Reservas.FindAsync(reservaActualizada.Id);
            if (reservaExistente is null) return;

            reservaExistente.Fecha = reservaActualizada.Fecha;
            reservaExistente.CanchaId = reservaActualizada.CanchaId;
            reservaExistente.HoraInicio = reservaActualizada.HoraInicio;
            reservaExistente.HoraFin = reservaActualizada.HoraFin;

            await _context.SaveChangesAsync();
        }


        public async Task EliminarReservaAsync(Reserva reserva)
        {
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Cancha>> ObtenerCanchasAsync()
        {
            return await _context.Canchas.ToListAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorDocumento(string documento)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Documento == documento);
        }
    }
}
