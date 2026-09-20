using ManualiAPI.Models;
using ManualiAPI.Repositories;

namespace ManualiAPI.Services;

public class ProductService : IProductService
{
    // Injeção de dependência: o service recebe o repository pronto (resolvido pelo .NET).
    // Nunca fazemos "new IProductRepository()" aqui.
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(ToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        // Validação de exemplo (regra de negócio mora aqui).
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("O nome do produto é obrigatório.");
        }

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            IsActive = dto.IsActive
        };

        var created = await _repository.AddAsync(product);
        return ToDto(created);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null)
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Price = dto.Price;
        existing.IsActive = dto.IsActive;

        var updated = await _repository.UpdateAsync(existing);
        return ToDto(updated);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _repository.DeleteAsync(id);
    }

    // Método privado: converte a entidade (banco) para DTO (retorno da API).
    private static ProductDto ToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            IsActive = product.IsActive
        };
    }
}