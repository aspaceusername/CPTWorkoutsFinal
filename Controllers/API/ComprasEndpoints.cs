using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CPTWorkouts.Controllers.API;

public static class ComprasEndpoints
{
    public static void MapComprasEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Compras").WithTags(nameof(Compras));

        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Compras.ToListAsync();
        })
        .WithName("GetAllCompras")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Compras>, NotFound>> (int clientefk, ApplicationDbContext db) =>
        {
            return await db.Compras.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ClienteFK == clientefk)
                is Compras model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetComprasById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int clientefk, Compras compras, ApplicationDbContext db) =>
        {
            var affected = await db.Compras
                .Where(model => model.ClienteFK == clientefk)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.DataCompra, compras.DataCompra)
                    .SetProperty(m => m.ServicoFK, compras.ServicoFK)
                    .SetProperty(m => m.ClienteFK, compras.ClienteFK)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateCompras")
        .WithOpenApi();

        group.MapPost("/", async (Compras compras, ApplicationDbContext db) =>
        {
            db.Compras.Add(compras);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Compras/{compras.ClienteFK}",compras);
        })
        .WithName("CreateCompras")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int clientefk, ApplicationDbContext db) =>
        {
            var affected = await db.Compras
                .Where(model => model.ClienteFK == clientefk)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteCompras")
        .WithOpenApi();
    }
}
