using Microsoft.EntityFrameworkCore;
using gestionReservas.Models;
using gestionReservas.Components.Pages;
using System.Collections.Generic;

namespace gestionReservas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cancha> Canchas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
    }
}
