namespace gestionReservas.Services
{
    public class AuthService
    {
        public bool EstaAutenticado { get; private set; } = false;
        public string? Documento { get; private set; }
        public string? NombreUsuario { get; private set; }
        public string? Rol { get; private set; } // <- ✅ este es nuevo


        public void IniciarSesion(string documento, string nombre, string rol)
        {
            EstaAutenticado = true;
            Documento = documento;
            NombreUsuario = nombre;
            Rol = rol;
        }

        public void CerrarSesion()
        {
            EstaAutenticado = false;
            Documento = null;
            NombreUsuario = null;
            Rol = null;
        }
    }
}
