using CPTWorkouts.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CPTWorkouts.Data
{
    internal class DbInitializer
    {
        internal static async Task Initialize(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));
            ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));

            dbContext.Database.EnsureCreated();

            bool haAdicao = false;
            
            // Certificar a existência de roles
            if (!await roleManager.RoleExistsAsync("Cliente"))
            {
                await roleManager.CreateAsync(new IdentityRole("Cliente"));
            }

            if (!await roleManager.RoleExistsAsync("Treinador"))
            {
                await roleManager.CreateAsync(new IdentityRole("Treinador"));
            }

            // Criar e guardar utilizadores
            var hasher = new PasswordHasher<IdentityUser>();
            IdentityUser user1 = null;
            IdentityUser user2 = null;

            if (!dbContext.Users.Any())
            {
                user1 = new IdentityUser
                {
                    UserName = "email.seis@mail.pt",
                    NormalizedUserName = "EMAIL.SEIS@MAIL.PT",
                    Email = "email.seis@mail.pt",
                    NormalizedEmail = "EMAIL.SEIS@MAIL.PT",
                    EmailConfirmed = true,
                    SecurityStamp = "5ZPZEF6SBW7IU4M344XNLT4NN5RO4GRU",
                    ConcurrencyStamp = "c86d8254-dd50-44be-8561-d2d44d4bbb2f",
                    PasswordHash = hasher.HashPassword(null, "Aa0_aa")
                };

                user2 = new IdentityUser
                {
                    UserName = "email.sete@mail.pt",
                    NormalizedUserName = "EMAIL.SETE@MAIL.PT",
                    Email = "email.sete@mail.pt",
                    NormalizedEmail = "EMAIL.SETE@MAIL.PT",
                    EmailConfirmed = true,
                    SecurityStamp = "TW49PF6SBW7IU4M344XNLT4NN5RO4GRU",
                    ConcurrencyStamp = "d8254c86-dd50-44be-8561-d2d44d4bbb2f",
                    PasswordHash = hasher.HashPassword(null, "Aa0_aa")
                };

                await dbContext.Users.AddRangeAsync(user1, user2);
                await dbContext.SaveChangesAsync(); // Guardar os utilizadores para se poder obter os seus IDs
                haAdicao = true;

                // Atribuir roles aos utilizadores
                await userManager.AddToRoleAsync(user1, "Cliente");
                await userManager.AddToRoleAsync(user2, "Treinador");
            }

            var equipas = Array.Empty<Equipas>();
            if (!dbContext.Equipas.Any())
            {
                equipas = new[]
                {
                    new Equipas { Nome = "Equipa A", Logotipo = "noImage.jpg" },
                    new Equipas { Nome = "Equipa B", Logotipo = "noImage.jpg" }
                };
                await dbContext.Equipas.AddRangeAsync(equipas);
                await dbContext.SaveChangesAsync();
                haAdicao = true;
            }

            var clientes = Array.Empty<Clientes>();
            if (!dbContext.Clientes.Any())
            {
                clientes = new[]
                {
                    new Clientes { Nome = "Mário", DataNascimento = DateOnly.Parse("2000-12-15"), Telemovel = "", Equipa = equipas[0], NumCliente = 1, UserID = user1.Id },
                    new Clientes { Nome = "Joana", DataNascimento = DateOnly.Parse("2000-12-16"), Telemovel = "913456789", Equipa = equipas[0], NumCliente = 2, UserID = "2" },
                    new Clientes { Nome = "João", DataNascimento = DateOnly.Parse("1999-12-31"), Telemovel = "92345687", Equipa = equipas[0], NumCliente = 3, UserID = "3" },
                    new Clientes { Nome = "Maria", DataNascimento = DateOnly.Parse("2000-12-15"), Telemovel = "9612347", Equipa = equipas[1], NumCliente = 4, UserID = "4" },
                    new Clientes { Nome = "Ana", DataNascimento = DateOnly.Parse("2000-12-15"), Telemovel = "", Equipa = equipas[1], NumCliente = 5, UserID = "5"}
                };
                await dbContext.Clientes.AddRangeAsync(clientes);
                await dbContext.SaveChangesAsync();
                haAdicao = true;
            }

            var treinadores = Array.Empty<Treinadores>();
            if (!dbContext.Treinadores.Any())
            {
                treinadores = new[]
                {
                    new Treinadores { Nome = "João Mendes", DataNascimento = DateOnly.Parse("1970-04-10"), Telemovel = "919876543", UserID = user2.Id },
                    new Treinadores { Nome = "Maria Sousa", DataNascimento = DateOnly.Parse("1988-09-12"), Telemovel = "918076543", UserID = "1" }
                };
                await dbContext.Treinadores.AddRangeAsync(treinadores);
                await dbContext.SaveChangesAsync();
                haAdicao = true;
            }

            var servicos = Array.Empty<Servicos>();
            if (!dbContext.Servicos.Any())
            {
                servicos = new[]
                {
                    new Servicos { Nome = "Servico A", Preco = 49, ListaTreinadores = new List<Treinadores> { treinadores[0] } },
                    new Servicos { Nome = "Servico B", Preco = 25, ListaTreinadores = new List<Treinadores> { treinadores[0], treinadores[1] } },
                    new Servicos { Nome = "Servico C", Preco = 15, ListaTreinadores = new List<Treinadores> { treinadores[1] } },
                    new Servicos { Nome = "Servico D", Preco = 64, ListaTreinadores = new List<Treinadores> { treinadores[1] } }
                };
                await dbContext.Servicos.AddRangeAsync(servicos);
                await dbContext.SaveChangesAsync();
                haAdicao = true;
            }

            var loadedClientes = await dbContext.Clientes.ToListAsync();
            var loadedServicos = await dbContext.Servicos.ToListAsync();

            var compras = Array.Empty<Compras>();
            if (!dbContext.Compras.Any())
            {
                compras = new[]
                {
                    new Compras { DataCompra = DateTime.Now, ValorCompraAux = "100.20", ServicoFK = loadedServicos[0].Id, ClienteFK = loadedClientes[0].Id },
                    new Compras { DataCompra = DateTime.Now.AddDays(-1), ValorCompraAux = "200,15", ServicoFK = loadedServicos[1].Id, ClienteFK = loadedClientes[0].Id }
                };
                await dbContext.Compras.AddRangeAsync(compras);
                haAdicao = true;
            }

            try
            {
                if (haAdicao)
                {
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
