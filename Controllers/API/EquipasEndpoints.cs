using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Hosting;

namespace CPTWorkouts.Controllers.API
{
    public static class EquipasEndpoints
    {
        public static void MapEquipasEndpoints(this IEndpointRouteBuilder routes)
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

            group.MapPost("/", async Task<Results<Created<Equipas>, BadRequest>> (HttpRequest request, ApplicationDbContext db) =>
            {
                if (!request.HasFormContentType)
                {
                    return TypedResults.BadRequest();
                }

                var form = await request.ReadFormAsync();
                var nome = form["Nome"].ToString();
                var logotipoFile = form.Files.GetFile("ImagemLogo"); // assuming the frontend sends 'ImagemLogo'

                if (string.IsNullOrEmpty(nome) || logotipoFile == null)
                {
                    return TypedResults.BadRequest();
                }

                // Generate a unique filename for the uploaded file
                var logotipoFileName = Guid.NewGuid().ToString() + Path.GetExtension(logotipoFile.FileName);

                var equipa = new Equipas
                {
                    Nome = nome,
                    Logotipo = logotipoFileName
                };

                // Save the uploaded file to a directory
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagens"); // adjust the path as necessary
                var filePath = Path.Combine(uploadsFolder, logotipoFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await logotipoFile.CopyToAsync(stream);
                }

                db.Equipas.Add(equipa);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/api/Equipas/{equipa.Id}", equipa);
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
}
