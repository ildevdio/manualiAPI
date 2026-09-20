using ManualiAPI.Models;

namespace ManualiAPI.Services;

// Service = camada de REGRAS DE NEGÓCIO.
// O controller conversa com o service, e o service conversa com o repository.
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}