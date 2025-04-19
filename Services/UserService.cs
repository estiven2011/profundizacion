using gestionReservas.Data;
using gestionReservas.Models;
using Microsoft.EntityFrameworkCore;

namespace gestionReservas.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<bool> UsuarioExiste(string documento)
        {
            return await _context.Usuarios.AnyAsync(u => u.Documento == documento);

        }
        public async Task<bool> ValidarUsuarioAsync(string documento, string contraseña)
        {
            return await _context.Usuarios.AnyAsync(u => u.Documento == documento && u.Contraseña == contraseña);
        }

        public async Task<string?> ObtenerNombreAsync(string documento)
        {
            return await _context.Usuarios
                .Where(u => u.Documento == documento)
                .Select(u => u.Nombre)
                .FirstOrDefaultAsync();
        }



    }
}
