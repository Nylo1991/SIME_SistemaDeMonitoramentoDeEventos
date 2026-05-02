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
            modelBuilder.Entity<Eventos>().ToTable("Eventos");
        }
    }
}