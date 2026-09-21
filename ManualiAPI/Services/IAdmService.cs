using ManualiAPI.DTO;

namespace ManualiAPI.Services;

public interface IAdmService
{
    GetAdmDto Criar(CreateAdmDto dto);

    IEnumerable<GetAdmDto> Listar();

    GetAdmDto BuscarPorId(int id);

    GetAdmDto BuscarPorUsername(string username);

    GetAdmDto Atualizar(int id, UpdateAdmDto dto);

    void Deletar(int id);
}