using ManualiAPI.DTO;
using ManualiAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManualiAPI.Controllers;

[ApiController]
[Route("api/artesaos")]
public class ArtesaosController : ControllerBase
{
    private readonly IArtesaoService _service;

    public ArtesaosController(IArtesaoService service)
    {
        _service = service;
    }

    // GET: /api/artesaos
    [HttpGet]
    public ActionResult<IEnumerable<GetArtesaoDto>> GetAll()
    {
        return Ok(_service.Listar());
    }

    // GET: /api/artesaos/search?username=maria
    [HttpGet("search")]
    public ActionResult<GetArtesaoDto> GetByUsername([FromQuery] string username)
    {
        return Ok(_service.BuscarPorUsername(username));
    }

    // GET: /api/artesaos/5
    [HttpGet("{id:int}")]
    public ActionResult<GetArtesaoDto> GetById(int id)
    {
        return Ok(_service.BuscarPorId(id));
    }

    // POST: /api/artesaos
    [HttpPost]
    public ActionResult<GetArtesaoDto> Create([FromBody] CreateArtesaoDto dto)
    {
        var artesao = _service.Criar(dto);
        return CreatedAtAction(nameof(GetById), new { id = artesao.Id }, artesao);
    }

    // PUT: /api/artesaos/5
    [HttpPut("{id:int}")]
    public ActionResult<GetArtesaoDto> Update(int id, [FromBody] UpdateArtesaoDto dto)
    {
        return Ok(_service.Atualizar(id, dto));
    }

    // DELETE: /api/artesaos/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _service.Deletar(id);
        return NoContent();
    }
}