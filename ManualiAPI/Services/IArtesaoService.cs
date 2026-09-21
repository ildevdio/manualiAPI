using ManualiAPI.DTO;

namespace ManualiAPI.Services;

public interface IArtesaoService
{
    GetArtesaoDto Criar(CreateArtesaoDto dto);

    IEnumerable<GetArtesaoDto> Listar();

    GetArtesaoDto BuscarPorId(int id);

    GetArtesaoDto BuscarPorUsername(string username);

    GetArtesaoDto Atualizar(int id, UpdateArtesaoDto dto);

    void Deletar(int id);
}