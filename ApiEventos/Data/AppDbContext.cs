using Microsoft.EntityFrameworkCore;
using Shared;

namespace ApiEventos.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Eventos> Eventos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Eventos>().ToTable("Eventos");

            modelBuilder.Entity<Eventos>()
                .Property(e => e.Tipo_Evento)
                .IsRequired();

            modelBuilder.Entity<Eventos>()
                .Property(e => e.local_Evento)
                .IsRequired();
        }
    }
}