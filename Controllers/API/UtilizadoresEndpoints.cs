using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CPTWorkouts.Controllers.API;

public static class UtilizadoresEndpoints
{
    public static void MapUtilizadoresEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Utilizadores").WithTags(nameof(Utilizadores));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Utilizadores.ToListAsync();
        })
        .WithName("GetAllUtilizadores")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Utilizadores>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Utilizadores.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Utilizadores model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUtilizadoresById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Utilizadores utilizadores, ApplicationDbContext db) =>
        {
            var affected = await db.Utilizadores
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, utilizadores.Id)
                    .SetProperty(m => m.Nome, utilizadores.Nome)
                    .SetProperty(m => m.DataNascimento, utilizadores.DataNascimento)
                    .SetProperty(m => m.Telemovel, utilizadores.Telemovel)
                    .SetProperty(m => m.UserID, utilizadores.UserID)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUtilizadores")
        .WithOpenApi();

        group.MapPost("/", async (Utilizadores utilizadores, ApplicationDbContext db) =>
        {
            db.Utilizadores.Add(utilizadores);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Utilizadores/{utilizadores.Id}",utilizadores);
        })
        .WithName("CreateUtilizadores")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Utilizadores
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUtilizadores")
        .WithOpenApi();
    }
}
