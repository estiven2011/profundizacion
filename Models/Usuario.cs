using System.ComponentModel.DataAnnotations;

namespace gestionReservas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El documento es obligatorio.")]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contraseña { get; set; } = string.Empty;
    }


}
