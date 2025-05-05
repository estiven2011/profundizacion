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
            reserva.Estado = "Activa"; // 🟢 Nuevo: todas las reservas nuevas son activas
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
            reservaExistente.Estado = reservaActualizada.Estado;

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

        public async Task CancelarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas.FindAsync(reservaId);
            if (reserva is null) return;

            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();
        }

        // ✅ NUEVO: Método para obtener historial de reservas por documento
        public async Task<List<Reserva>> ObtenerReservasPorDocumentoAsync(string documento)
        {
            return await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Usuario)
                .Where(r => r.Usuario.Documento == documento)
                .ToListAsync();
        }

        //NUEVO
        public async Task<List<Reserva>> ObtenerReservasPorCanchaYFecha(int canchaId, DateTime fecha)
        {
            return await _context.Reservas
                .Where(r => r.CanchaId == canchaId && r.Fecha == fecha && r.Estado == "Activa")
                .ToListAsync();
        }

    }
}
