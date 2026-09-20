using ManualiAPI.Exceptions;
using ManualiAPI.DTO;
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
    public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAll()
    {
        var products = _service.Listar();
        return Ok(products); // 200 OK
    }

    // GET: /api/products/search?nome=caneca
    [HttpGet("search")]
    public ActionResult<IEnumerable<GetProductDto>> SearchByName([FromQuery] string nome)
    {
        var products = _service.BuscarPorNome(nome);
        return Ok(products);
    }

    // GET: /api/products/5
    [HttpGet("{id}")]
    public ActionResult<GetProductDto> GetById(int id)
    {
        try
        {
            var product = _service.BuscarPorId(id);
            return Ok(product);
        }
        catch (ProductNotFoundException)
        {
            return NotFound(); // 404
        }
    }

    // POST: /api/products
    [HttpPost]
    public ActionResult<GetProductDto> Create([FromBody] CreateProductDto dto)
    {
        try
        {
            var product = _service.Criar(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400
        }
    }

    // PUT: /api/products/5
    [HttpPut("{id}")]
    public ActionResult<GetProductDto> Update(int id, [FromBody] UpdateProductDto dto)
    {
        try
        {
            var product = _service.Atualizar(id, dto);
            return Ok(product);
        }
        catch (ProductNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: /api/products/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.Deletar(id);
            return NoContent(); // 204
        }
        catch (ProductNotFoundException)
        {
            return NotFound();
        }
    }
}