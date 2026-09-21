using ManualiAPI.DTO;
using ManualiAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManualiAPI.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;

    public UsuariosController(IUsuarioService service)
    {
        _service = service;
    }

    // GET: /api/usuarios
    [HttpGet]
    public ActionResult<IEnumerable<GetUsuarioDto>> GetAll()
    {
        return Ok(_service.Listar());
    }

    // GET: /api/usuarios/search?username=ana
    [HttpGet("search")]
    public ActionResult<GetUsuarioDto> GetByUsername([FromQuery] string username)
    {
        return Ok(_service.BuscarPorUsername(username));
    }

    // GET: /api/usuarios/5
    [HttpGet("{id:int}")]
    public ActionResult<GetUsuarioDto> GetById(int id)
    {
        return Ok(_service.BuscarPorId(id));
    }

    // POST: /api/usuarios
    [HttpPost]
    public ActionResult<GetUsuarioDto> Create([FromBody] CreateUsuarioDto dto)
    {
        var usuario = _service.Criar(dto);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    // PUT: /api/usuarios/5
    [HttpPut("{id:int}")]
    public ActionResult<GetUsuarioDto> Update(int id, [FromBody] UpdateUsuarioDto dto)
    {
        return Ok(_service.Atualizar(id, dto));
    }

    // DELETE: /api/usuarios/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _service.Deletar(id);
        return NoContent();
    }
}