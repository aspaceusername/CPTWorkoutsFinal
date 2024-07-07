using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CPTWorkouts.Controllers.API;

public static class EquipasEndpoints
{
    public static void MapEquipasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Equipas").WithTags(nameof(Equipas));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Equipas.ToListAsync();
        })
        .WithName("GetAllEquipas")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Equipas>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Equipas.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Equipas model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetEquipasById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Equipas equipas, ApplicationDbContext db) =>
        {
            var affected = await db.Equipas
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, equipas.Id)
                    .SetProperty(m => m.Nome, equipas.Nome)
                    .SetProperty(m => m.Logotipo, equipas.Logotipo)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateEquipas")
        .WithOpenApi();

        group.MapPost("/", async (Equipas equipas, ApplicationDbContext db) =>
        {
            db.Equipas.Add(equipas);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Equipas/{equipas.Id}",equipas);
        })
        .WithName("CreateEquipas")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Equipas
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteEquipas")
        .WithOpenApi();
    }
}
