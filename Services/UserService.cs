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
            try
            {
                usuario.Rol = "Cliente";
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR AL GUARDAR USUARIO: " + ex.Message);
                throw;
            }
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

        // NUEVO 
        public async Task<string?> ObtenerRolAsync(string documento)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Documento == documento);
            return usuario?.Rol;
        }

        // NUEVO
        public async Task<int?> ObtenerIdPorDocumentoAsync(string documento)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Documento == documento);
            return usuario?.Id;
        }

    }
}
