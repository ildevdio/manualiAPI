using ManualiAPI.DTO;

namespace ManualiAPI.Services;

public interface IUsuarioService
{
    GetUsuarioDto Criar(CreateUsuarioDto dto);

    IEnumerable<GetUsuarioDto> Listar();

    GetUsuarioDto BuscarPorId(int id);

    GetUsuarioDto BuscarPorUsername(string username);

    GetUsuarioDto Atualizar(int id, UpdateUsuarioDto dto);

    void Deletar(int id);
}