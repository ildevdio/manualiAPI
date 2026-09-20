using ManualiAPI.Models;
using ManualiAPI.Services;

namespace ManualiAPI.Routes;

// Minimal API: alternativa aos Controllers, tudo declarado como lambdas.
// Permite criar endpoints com menos código boilerplate.
public static class ProductRoutes
{
    // Método de extensão chamado no Program.cs: app.MapProductRoutes();
    public static void MapProductRoutes(this WebApplication app)
    {
        var group = app.MapGroup("/api/products-minimal").WithTags("Products (Minimal)");

        // GET: /api/products-minimal
        group.MapGet("/", async (IProductService service) =>
        {
            var products = await service.GetAllAsync();
            return Results.Ok(products);
        });

        // GET: /api/products-minimal/5
        // O parâmetro "id" é resolvido automaticamente pela rota.
        group.MapGet("/{id}", async (int id, IProductService service) =>
        {
            var product = await service.GetByIdAsync(id);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        // POST: /api/products-minimal  (o corpo JSON vira CreateProductDto automaticamente)
        group.MapPost("/", async (CreateProductDto dto, IProductService service) =>
        {
            var product = await service.CreateAsync(dto);
            return Results.Created($"/api/products-minimal/{product.Id}", product);
        });

        // PUT: /api/products-minimal/5
        group.MapPut("/{id}", async (int id, UpdateProductDto dto, IProductService service) =>
        {
            var product = await service.UpdateAsync(id, dto);
            return product is null ? Results.NotFound() : Results.Ok(product);
        });

        // DELETE: /api/products-minimal/5
        group.MapDelete("/{id}", async (int id, IProductService service) =>
        {
            var removed = await service.DeleteAsync(id);
            return removed ? Results.NoContent() : Results.NotFound();
        });
    }
}