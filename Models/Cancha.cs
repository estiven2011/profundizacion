using System.ComponentModel.DataAnnotations;

namespace gestionReservas.Models
{
    public class Cancha
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Ubicacion { get; set; }

        public decimal PrecioPorHora { get; set; }
    }
}
