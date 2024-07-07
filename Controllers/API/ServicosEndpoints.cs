using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CPTWorkouts.Controllers.API;

public static class ServicosEndpoints
{
    public static void MapServicosEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Servicos").WithTags(nameof(Servicos));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Servicos.ToListAsync();
        })
        .WithName("GetAllServicos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Servicos>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Servicos.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Servicos model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetServicosById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Servicos servicos, ApplicationDbContext db) =>
        {
            var affected = await db.Servicos
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, servicos.Id)
                    .SetProperty(m => m.Nome, servicos.Nome)
                    .SetProperty(m => m.Preco, servicos.Preco)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateServicos")
        .WithOpenApi();

        group.MapPost("/", async (Servicos servicos, ApplicationDbContext db) =>
        {
            db.Servicos.Add(servicos);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Servicos/{servicos.Id}",servicos);
        })
        .WithName("CreateServicos")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Servicos
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteServicos")
        .WithOpenApi();
    }
}
