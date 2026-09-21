using ManualiAPI.DTO;
using ManualiAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManualiAPI.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidosController(IPedidoService service)
    {
        _service = service;
    }

    // GET: /api/pedidos
    [HttpGet]
    public ActionResult<IEnumerable<GetPedidoDto>> GetAll()
    {
        return Ok(_service.Listar());
    }

    // GET: /api/pedidos/5
    [HttpGet("{id:int}")]
    public ActionResult<GetPedidoDto> GetById(int id)
    {
        return Ok(_service.BuscarPorId(id));
    }

    // POST: /api/pedidos
    [HttpPost]
    public ActionResult<GetPedidoDto> Create([FromBody] CreatePedidoDto dto)
    {
        var pedido = _service.Criar(dto);
        return CreatedAtAction(nameof(GetById), new { id = pedido.IdPedido }, pedido);
    }

    // PUT: /api/pedidos/5
    [HttpPut("{id:int}")]
    public ActionResult<GetPedidoDto> Update(int id, [FromBody] UpdatePedidoDto dto)
    {
        return Ok(_service.Atualizar(id, dto));
    }

    // DELETE: /api/pedidos/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _service.Deletar(id);
        return NoContent();
    }
}