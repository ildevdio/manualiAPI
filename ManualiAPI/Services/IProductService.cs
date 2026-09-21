using ManualiAPI.DTO;

namespace ManualiAPI.Services;

public interface IProductService
{
    GetProductDto Criar(CreateProductDto dto);
    IEnumerable<GetProductDto> Listar();
    GetProductDto BuscarPorId(int id);
    IEnumerable<GetProductDto> BuscarPorNome(string nome);
    GetProductDto Atualizar(int id, UpdateProductDto dto);
    void Deletar(int id);
    IReadOnlyList<(int IdProduto, int Quantidade, decimal PrecoUnitario)> ReservarEstoque(
        IEnumerable<(int IdProduto, int Quantidade)> itens);
    void DevolverEstoque(IEnumerable<(int IdProduto, int Quantidade)> itens);
}