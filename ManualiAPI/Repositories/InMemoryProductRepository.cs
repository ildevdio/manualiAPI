using ManualiAPI.Models;

namespace ManualiAPI.Repositories;

// Implementação em memória da interface, para a equipe começar sem precisar de banco.
// Quando for usar banco de verdade, basta criar outra classe (ex.: ProductRepositoryEFCore)
// que também implemente IProductRepository e trocar a linha no Program.cs.
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();
    private int _nextId = 1;

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        return Task.FromResult(_products.AsEnumerable());
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        // Find retorna Product ou null (por isso o "?" no tipo de retorno).
        return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    }

    public Task<Product> AddAsync(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return Task.FromResult(product);
    }

    public Task<Product> UpdateAsync(Product product)
    {
        var existing = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existing is null)
        {
            throw new KeyNotFoundException($"Produto {product.Id} não encontrado.");
        }

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.IsActive = product.IsActive;

        return Task.FromResult(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is null)
        {
            return Task.FromResult(false);
        }

        _products.Remove(existing);
        return Task.FromResult(true);
    }
}