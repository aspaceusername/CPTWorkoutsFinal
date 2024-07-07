using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CPTWorkouts.Controllers.API;

public static class TreinadoresEndpoints
{
    public static void MapTreinadoresEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Treinadores").WithTags(nameof(Treinadores));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Treinadores.ToListAsync();
        })
        .WithName("GetAllTreinadores")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Treinadores>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Treinadores.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Treinadores model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTreinadoresById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Treinadores treinadores, ApplicationDbContext db) =>
        {
            var affected = await db.Treinadores
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, treinadores.Id)
                    .SetProperty(m => m.Nome, treinadores.Nome)
                    .SetProperty(m => m.DataNascimento, treinadores.DataNascimento)
                    .SetProperty(m => m.Telemovel, treinadores.Telemovel)
                    .SetProperty(m => m.UserID, treinadores.UserID)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTreinadores")
        .WithOpenApi();

        group.MapPost("/", async (Treinadores treinadores, ApplicationDbContext db) =>
        {
            db.Treinadores.Add(treinadores);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Treinadores/{treinadores.Id}",treinadores);
        })
        .WithName("CreateTreinadores")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Treinadores
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTreinadores")
        .WithOpenApi();
    }
}
