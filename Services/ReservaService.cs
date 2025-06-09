using gestionReservas.Data;
using gestionReservas.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionReservas.Services
{
    public class ReservaService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ReservaService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
            reserva.Estado = "Activa";
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Enviar correo al crear
            var usuario = await _context.Usuarios.FindAsync(reserva.UsuarioId);
            await _context.Entry(reserva).Reference(r => r.Cancha).LoadAsync();

            if (!string.IsNullOrWhiteSpace(usuario?.Correo))
            {
                string asunto = "Confirmación de reserva de cancha";
                string cuerpo = $"Hola {usuario.Nombre},\n\n" +
                                $"Tu reserva fue confirmada con éxito:\n\n" +
                                $"- Fecha: {reserva.Fecha:dddd, dd MMMM yyyy}\n" +
                                $"- Hora: {reserva.HoraInicio} a {reserva.HoraFin}\n" +
                                $"- Cancha: {reserva.Cancha?.Nombre ?? "N/A"}\n\n" +
                                $"Gracias por usar nuestro sistema de reservas.";

                await _emailService.EnviarCorreoAsync(usuario.Correo, asunto, cuerpo);
            }
        }

        public async Task EditarReservaAsync(Reserva reservaActualizada)
        {
            var reservaExistente = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Cancha)
                .FirstOrDefaultAsync(r => r.Id == reservaActualizada.Id);

            if (reservaExistente is null) return;

            reservaExistente.Fecha = reservaActualizada.Fecha;
            reservaExistente.CanchaId = reservaActualizada.CanchaId;
            reservaExistente.HoraInicio = reservaActualizada.HoraInicio;
            reservaExistente.HoraFin = reservaActualizada.HoraFin;
            reservaExistente.Estado = reservaActualizada.Estado;

            await _context.SaveChangesAsync();

            // Enviar correo al editar
            if (!string.IsNullOrWhiteSpace(reservaExistente.Usuario?.Correo))
            {
                string asunto = "Actualización de reserva de cancha";
                string cuerpo = $"Hola {reservaExistente.Usuario.Nombre},\n\n" +
                                $"Tu reserva ha sido actualizada con éxito:\n\n" +
                                $"- Fecha: {reservaExistente.Fecha:dddd, dd MMMM yyyy}\n" +
                                $"- Hora: {reservaExistente.HoraInicio} a {reservaExistente.HoraFin}\n" +
                                $"- Cancha: {reservaExistente.Cancha?.Nombre ?? "N/A"}\n\n" +
                                $"Gracias por confiar en nosotros.";

                await _emailService.EnviarCorreoAsync(reservaExistente.Usuario.Correo, asunto, cuerpo);
            }
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

        public async Task<List<Reserva>> ObtenerReservasPorDocumentoAsync(string documento)
        {
            return await _context.Reservas
                .Include(r => r.Cancha)
                .Include(r => r.Usuario)
                .Where(r => r.Usuario.Documento == documento)
                .ToListAsync();
        }

        public async Task<List<Reserva>> ObtenerReservasPorCanchaYFecha(int canchaId, DateTime fecha)
        {
            return await _context.Reservas
                .Where(r => r.CanchaId == canchaId && r.Fecha == fecha && r.Estado == "Activa")
                .ToListAsync();
        }
    }
}
