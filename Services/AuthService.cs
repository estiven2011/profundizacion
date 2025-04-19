namespace gestionReservas.Services
{
    public class AuthService
    {
        public bool EstaAutenticado { get; private set; } = false;
        public string? Documento { get; private set; }
        public string? NombreUsuario { get; private set; }

        public void IniciarSesion(string documento, string nombre)
        {
            EstaAutenticado = true;
            Documento = documento;
            NombreUsuario = nombre;
        }

        public void CerrarSesion()
        {
            EstaAutenticado = false;
            Documento = null;
            NombreUsuario = null;
        }
    }
}
