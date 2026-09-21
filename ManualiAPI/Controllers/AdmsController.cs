using ManualiAPI.DTO;
using ManualiAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManualiAPI.Controllers;

[ApiController]
[Route("api/adms")]
public class AdmsController : ControllerBase
{
    private readonly IAdmService _service;

    public AdmsController(IAdmService service)
    {
        _service = service;
    }

    // GET: /api/adms
    [HttpGet]
    public ActionResult<IEnumerable<GetAdmDto>> GetAll()
    {
        return Ok(_service.Listar());
    }

    // GET: /api/adms/search?username=joao
    [HttpGet("search")]
    public ActionResult<GetAdmDto> GetByUsername([FromQuery] string username)
    {
        return Ok(_service.BuscarPorUsername(username));
    }

    // GET: /api/adms/5
    [HttpGet("{id:int}")]
    public ActionResult<GetAdmDto> GetById(int id)
    {
        return Ok(_service.BuscarPorId(id));
    }

    // POST: /api/adms
    [HttpPost]
    public ActionResult<GetAdmDto> Create([FromBody] CreateAdmDto dto)
    {
        var adm = _service.Criar(dto);
        return CreatedAtAction(nameof(GetById), new { id = adm.IdAdm }, adm);
    }

    // PUT: /api/adms/5
    [HttpPut("{id:int}")]
    public ActionResult<GetAdmDto> Update(int id, [FromBody] UpdateAdmDto dto)
    {
        return Ok(_service.Atualizar(id, dto));
    }

    // DELETE: /api/adms/5
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _service.Deletar(id);
        return NoContent();
    }
}