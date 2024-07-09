using Microsoft.EntityFrameworkCore;
using CPTWorkouts.Data;
using CPTWorkouts.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
namespace CPTWorkouts.Controllers.API;

public static class ClientesEndpoints
{
    public static void MapClientesEndpoints (this IEndpointRouteBuilder routes)
    {
        //Esta linha cria um grupo de rotas com o caminho base /api/Clientes e marca-o com o nome Clientes
        var group = routes.MapGroup("/api/Clientes").WithTags(nameof(Clientes));

        //Este endpoint responde a pedidos GET /api/Clientes. Ele recupera todos os registos de Clientes da base de dados e retorna-os como uma lista.
        group.MapGet("/", async (ApplicationDbContext db) =>
        {
            return await db.Clientes.ToListAsync();
        })
        .WithName("GetAllClientes")
        .WithOpenApi();

        //Este endpoint responde a pedidos GET /api/Clientes/{id}.
        //Procura um registo de Clientes pelo seu ID. Se encontrado, retorna o registo; caso contrário, retorna um resultado NotFound.
        group.MapGet("/{id}", async Task<Results<Ok<Clientes>, NotFound>> (int id, ApplicationDbContext db) =>
        {
            return await db.Clientes.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Clientes model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetClientesById")
        .WithOpenApi();

        //Este endpoint responde a pedidos PUT /api/Clientes/{id}. Ele atualiza um registo existente de Clientes com os dados fornecidos.
        //Se a atualização for bem-sucedida, retorna Ok; caso contrário, retorna NotFound.
        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Clientes clientes, ApplicationDbContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.NumCliente, clientes.NumCliente)
                    .SetProperty(m => m.EquipaFK, clientes.EquipaFK)
                    .SetProperty(m => m.Id, clientes.Id)
                    .SetProperty(m => m.Nome, clientes.Nome)
                    .SetProperty(m => m.DataNascimento, clientes.DataNascimento)
                    .SetProperty(m => m.Telemovel, clientes.Telemovel)
                    .SetProperty(m => m.UserID, clientes.UserID)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateClientes")
        .WithOpenApi();

        //Este endpoint responde a pedidos POST /api/Clientes.
        //Adiciona um novo registo de Clientes à base de dados e retorna o registo criado com a sua localização.
        group.MapPost("/", async (Clientes clientes, ApplicationDbContext db) =>
        {
            db.Clientes.Add(clientes);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Clientes/{clientes.Id}",clientes);
        })
        .WithName("CreateClientes")
        .WithOpenApi();

        //Este endpoint responde a pedidos DELETE /api/Clientes/{id}.
        //Apaga um registo de Clientes pelo seu ID. Se a eliminação for bem-sucedida, retorna Ok; caso contrário, retorna NotFound.
        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, ApplicationDbContext db) =>
        {
            var affected = await db.Clientes
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteClientes")
        .WithOpenApi();
    }
}
