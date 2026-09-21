using ManualiAPI.DTO;

namespace ManualiAPI.Services;

public interface IPedidoService
{
    GetPedidoDto Criar(CreatePedidoDto dto);
    IEnumerable<GetPedidoDto> Listar();
    GetPedidoDto BuscarPorId(int id);
    GetPedidoDto Atualizar(int id, UpdatePedidoDto dto);
    void Deletar(int id);
}