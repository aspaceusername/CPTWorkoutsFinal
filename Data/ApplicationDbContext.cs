using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Models;

namespace CPTWorkouts.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CPTWorkouts.Models.Clientes> Clientes { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Equipas> Equipas { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Servicos> Servicos { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Utilizadores> Utilizadores { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Treinadores> Treinadores { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Compras> Compras { get; set; } = default!;
    }
}
