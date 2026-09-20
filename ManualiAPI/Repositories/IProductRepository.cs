using ManualiAPI.Models;

namespace ManualiAPI.Repositories;

// Interface: "contrato" que diz O QUE o repositório faz, sem dizer COMO.
// Depois criamos classes concretas que implementam essa interface (ex.: em memória, SQL, MongoDB).
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
}