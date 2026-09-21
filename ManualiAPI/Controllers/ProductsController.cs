using ManualiAPI.DTO;
using ManualiAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManualiAPI.Controllers;

// Controller = camada de APRESENTAÇÃO (HTTP).
// Recebe a requisição, chama o service e devolve IActionResult (status code + corpo).
// Erros de negócio (404, 409, 400 etc.) são tratados pelo ExceptionHandlingMiddleware,
// que devolve sempre { "erro": "..." }.
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
    public ActionResult<IEnumerable<GetProductDto>> GetAll()
    {
        return Ok(_service.Listar()); // 200 OK
    }

    // GET: /api/products/search?nome=caneca
    [HttpGet("search")]
    public ActionResult<IEnumerable<GetProductDto>> SearchByName([FromQuery] string nome)
    {
        return Ok(_service.BuscarPorNome(nome));
    }

    // GET: /api/products/5
    [HttpGet("{id:int}")]
    public ActionResult<GetProductDto> GetById(int id)
    {
        return Ok(_service.BuscarPorId(id));
    }

    // POST: /api/products
    [HttpPost]
    public ActionResult<GetProductDto> Create([FromBody] CreateProductDto dto)
    {
        var product = _service.Criar(dto);
        // 201 Created + cabeçalho Location apontando para o recurso criado.
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT: /api/products/5
    [HttpPut("{id:int}")]
    public ActionResult<GetProductDto> Update(int id, [FromBody] UpdateProductDto dto)
    {
        return Ok(_service.Atualizar(id, dto));
    }

    // DELETE: /api/products/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _service.Deletar(id);
        return NoContent(); // 204
    }
}