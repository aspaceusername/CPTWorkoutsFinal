using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Routing;

namespace CPTWorkouts.Controllers.API
{
    public static class ClientesEndpoints
    {
        public static void MapClientesEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/Clientes").WithTags(nameof(Clientes));

            group.MapGet("/", async (ApplicationDbContext db) =>
            {
                return await db.Clientes.ToListAsync();
            })
            .WithName("GetAllClientes")
            .WithOpenApi();

            group.MapGet("/{id}", async Task<Results<Ok<Clientes>, NotFound>> (int id, ApplicationDbContext db) =>
            {
                var cliente = await db.Clientes.AsNoTracking()
                    .FirstOrDefaultAsync(model => model.Id == id);

                if (cliente == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(cliente);
            })
            .WithName("GetClientesById")
            .WithOpenApi();

            group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Clientes clientes, ApplicationDbContext db) =>
            {
                var existingCliente = await db.Clientes.FindAsync(id);

                if (existingCliente == null)
                {
                    return TypedResults.NotFound();
                }

                existingCliente.NumCliente = clientes.NumCliente;
                existingCliente.EquipaFK = clientes.EquipaFK;
                existingCliente.Nome = clientes.Nome;
                existingCliente.DataNascimento = clientes.DataNascimento;
                existingCliente.Telemovel = clientes.Telemovel;
                existingCliente.UserID = clientes.UserID;

                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithName("UpdateClientes")
            .WithOpenApi();

            group.MapPost("/", async Task<Results<Created<Clientes>, BadRequest>> (Clientes clientes, ApplicationDbContext db) =>
            {
                if (clientes == null)
                {
                    return TypedResults.BadRequest();
                }

                try
                {
                    db.Clientes.Add(clientes);
                    await db.SaveChangesAsync();

                    return TypedResults.Created($"/api/Clientes/{clientes.Id}", clientes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding cliente: {ex.Message}");
                    return TypedResults.BadRequest();
                }
            })
            .WithName("CreateClientes")
            .WithOpenApi();

            group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
            {
                var existingCliente = await db.Clientes.FindAsync(id);

                if (existingCliente == null)
                {
                    return TypedResults.NotFound();
                }

                db.Clientes.Remove(existingCliente);
                await db.SaveChangesAsync();

                return TypedResults.Ok();
            })
            .WithName("DeleteClientes")
            .WithOpenApi();
        }
    }
}
