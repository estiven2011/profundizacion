namespace gestionReservas.Services
{
    using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

    public class AuthService
    {
        private readonly ProtectedSessionStorage _sessionStorage;

        public bool EstaAutenticado { get; private set; } = false;
        public string? Documento { get; private set; }
        public string? NombreUsuario { get; private set; }
        public string? Rol { get; private set; }

        public AuthService(ProtectedSessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public async Task IniciarSesion(string documento, string nombre, string rol)
        {
            EstaAutenticado = true;
            Documento = documento;
            NombreUsuario = nombre;
            Rol = rol;

            // Guardar en session storage
            await _sessionStorage.SetAsync("userData", new UserSessionData
            {
                Documento = documento,
                Nombre = nombre,
                Rol = rol
            });
        }

        public async Task CerrarSesion()
        {
            EstaAutenticado = false;
            Documento = null;
            NombreUsuario = null;
            Rol = null;

            await _sessionStorage.DeleteAsync("userData");
        }

        public async Task VerificarSesion()
        {
            var result = await _sessionStorage.GetAsync<UserSessionData>("userData");

            if (result.Success && result.Value is not null)
            {
                Documento = result.Value.Documento;
                NombreUsuario = result.Value.Nombre;
                Rol = result.Value.Rol;
                EstaAutenticado = true;
            }
        }

        public class UserSessionData
        {
            public string Documento { get; set; }
            public string Nombre { get; set; }
            public string Rol { get; set; }
        }
    }

}
