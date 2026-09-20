using ManualiAPI.Models;
using ManualiAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManualiAPI.Controllers;

// Controller = camada de APRESENTAÇÃO (HTTP).
// Recebe a requisição, chama o service e devolve IActionResult (status code + corpo).
[ApiController]                 // Habilita validação automática de modelo e inferência de rota.
[Route("api/products")]         // Prefixo das rotas deste controller.
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    // GET: /api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        var products = await _service.GetAllAsync();
        return Ok(products); // 200 OK
    }

    // GET: /api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound(); // 404
        }

        return Ok(product);
    }

    // POST: /api/products  (corpo da requisição vem no JSON)
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var product = await _service.CreateAsync(dto);
            // 201 Created + cabeçalho Location apontando para o recurso criado.
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400
        }
    }

    // PUT: /api/products/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = await _service.UpdateAsync(id, dto);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    // DELETE: /api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var removed = await _service.DeleteAsync(id);
        if (!removed)
        {
            return NotFound();
        }

        return NoContent(); // 204 No Content
    }
}