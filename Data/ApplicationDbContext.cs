using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Identity;

namespace CPTWorkouts.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


      protected override void OnModelCreating(ModelBuilder builder) {
         /* Esta instrução importa tudo o que está pre-definido
          * na super classe
          */
         base.OnModelCreating(builder);

         /* Adição de dados à Base de Dados
          * Esta forma é PERSISTENTE, pelo que apenas deve ser utilizada em 
          * dados que perduram da fase de 'desenvolvimento' para a fase de 'produção'.
          * Implica efetuar um 'Add-Migration'
          * 
          * Atribuir valores às ROLES
          */
         builder.Entity<IdentityRole>().HasData(
             new IdentityRole { Id = "t", Name = "Treinador", NormalizedName = "TREINADOR" },
             new IdentityRole { Id = "cl", Name = "Cliente", NormalizedName = "CLIENTE" }
             );

      }
        public DbSet<CPTWorkouts.Models.Clientes> Clientes { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Equipas> Equipas { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Servicos> Servicos { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Utilizadores> Utilizadores { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Treinadores> Treinadores { get; set; } = default!;
        public DbSet<CPTWorkouts.Models.Compras> Compras { get; set; } = default!;


    }
}
