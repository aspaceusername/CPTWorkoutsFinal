using CPTWorkouts.Models;

using Microsoft.AspNetCore.Identity;


namespace CPTWorkouts.Data
{

    internal class DbInitializer
    {

        internal static async void Initialize(ApplicationDbContext dbContext)
        {

            /*
             * https://stackoverflow.com/questions/70581816/how-to-seed-data-in-net-core-6-with-entity-framework
             * 
             * https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-6.0#initialize-db-with-test-data
             * https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/data/ef-mvc/intro/samples/5cu/Program.cs
             * https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0300
             */


            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
            dbContext.Database.EnsureCreated();

            // var auxiliar
            bool haAdicao = false;



            // Se não houver Cursos, cria-os
            var equipas = Array.Empty<Equipas>();
            if (!dbContext.Equipas.Any())
            {
                equipas = [
                   new Equipas{ Nome="Equipa A", Logotipo="noImage.jpg" },
                   new Equipas{ Nome="Equipa B", Logotipo="noImage.jpg" }
                //adicionar outros cursos
                ];
                await dbContext.Equipas.AddRangeAsync(equipas);
                haAdicao = true;
            }


            // Se não houver Clientes, cria-os
            var clientes = Array.Empty<Clientes>();
            if (!dbContext.Clientes.Any())
            {
                clientes = [
                   new Clientes{ Nome="Mário", DataNascimento=DateOnly.Parse("2000-12-15"),Telemovel="" ,
                           Equipa= equipas[0], DataCompra=DateTime.Parse("2024-02-15"), NumCliente=1 },
               new Clientes{ Nome="Joana", DataNascimento=DateOnly.Parse("2000-12-16"),Telemovel="913456789" ,
                           Equipa= equipas[0], DataCompra=DateTime.Parse("2024-12-15"), NumCliente=2 },
               new Clientes{ Nome="João", DataNascimento=DateOnly.Parse("1999-12-31"),Telemovel="92345687" ,
                           Equipa= equipas[0], DataCompra=DateTime.Parse("2024-12-15"), NumCliente=3 },

               new Clientes{ Nome="Maria", DataNascimento=DateOnly.Parse("2000-12-15"),Telemovel="9612347" ,
                           Equipa= equipas[1], DataCompra=DateTime.Parse("2024-12-15"), NumCliente=4 },
               new Clientes{ Nome="Ana", DataNascimento=DateOnly.Parse("2000-12-15"),Telemovel="" ,
                           Equipa= equipas[1], DataCompra=DateTime.Parse("2024-12-15"), NumCliente=5 },
               //add other users
            ];
                await dbContext.Clientes.AddRangeAsync(clientes);
                haAdicao = true;
            }


            // Se não houver Utilizadores Identity, cria-os
            var users = Array.Empty<IdentityUser>();
            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            if (!dbContext.Users.Any())
            {
                users = [
                   new IdentityUser{UserName="email.seis@mail.pt", NormalizedUserName="EMAIL.SEIS@MAIL.PT",
               Email="email.seis@mail.pt",NormalizedEmail="EMAIL.SEIS@MAIL.PT", EmailConfirmed=true,
               SecurityStamp="5ZPZEF6SBW7IU4M344XNLT4NN5RO4GRU", ConcurrencyStamp="c86d8254-dd50-44be-8561-d2d44d4bbb2f",
               PasswordHash=hasher.HashPassword(null,"Aa0_aa") },
            new IdentityUser{UserName="email.sete@mail.pt", NormalizedUserName="EMAIL.SETE@MAIL.PT",
               Email="email.sete@mail.pt",NormalizedEmail="EMAIL.SETE@MAIL.PT", EmailConfirmed=true,
               SecurityStamp="TW49PF6SBW7IU4M344XNLT4NN5RO4GRU", ConcurrencyStamp="d8254c86-dd50-44be-8561-d2d44d4bbb2f",
               PasswordHash=hasher.HashPassword(null,"Aa0_aa") }
                   ];
                await dbContext.Users.AddRangeAsync(users);
                haAdicao = true;
            }


            // Se não houver Treinadores, cria-os
            var treinadores = Array.Empty<Treinadores>();
            if (!dbContext.Treinadores.Any())
            {
                treinadores = [
                   new Treinadores { Nome="João Mendes", DataNascimento=DateOnly.Parse("1970-04-10"), Telemovel="919876543" , UserID=users[0].Id },
               new Treinadores { Nome="Maria Sousa", DataNascimento=DateOnly.Parse("1988-09-12"), Telemovel="918076543" , UserID=users[1].Id }
                  ];
                await dbContext.Treinadores.AddRangeAsync(treinadores);
                haAdicao = true;
            }


            // Se não houver UCs, cria-as
            var servicos = Array.Empty<Servicos>();
            if (!dbContext.Servicos.Any())
            {
                servicos = [
                   new Servicos{Nome="Servico A", Preco=49, ListaTreinadores=[ treinadores[0] ]},
               new Servicos{Nome="Servico B",Preco=25, ListaTreinadores=[treinadores[0], treinadores[1] ]},
               new Servicos{Nome="Servico C", Preco=15, ListaTreinadores=[treinadores[1] ]},
               new Servicos{Nome="Servico D", Preco=64,ListaTreinadores=[treinadores[1] ]}
                ];
                await dbContext.Servicos.AddRangeAsync(servicos);
                haAdicao = true;
            }


            try
            {
                if (haAdicao)
                {
                    // tornar persistentes os dados
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
    }
}