namespace ManualiAPI.Services;

using ManualiAPI.DTO;
using System.Collections.Generic;

public interface IProductService
{
    GetProductDto Criar(CreateProductDto dto);
    
    IEnumerable<GetProductDto> Listar();

    GetProductDto BuscarPorId(int id);
    
    GetProductDto BuscarPorNome(string nome);
    
    GetProductDto Atualizar(int id, UpdateProductDto dto);
    
    void Deletar(int id);
    
    
}