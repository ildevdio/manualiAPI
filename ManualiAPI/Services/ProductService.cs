using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;

namespace ManualiAPI.Services;

public class ProductService : IProductService
{
    private readonly ConcurrentDictionary<int, Product> _products = new();

    public GetProductDto Criar(CreateProductDto dto)
    {
        Product product = new Product(dto.Nome, dto.Descricao, dto.Preco, dto.Estoque, dto.Ativo);

        _products[product.Id] = product;

        return new GetProductDto
        {
            Id = product.Id,
            Nome = product.Nome,
            Preco = product.Preco,
            Estoque = product.Estoque,
            Ativo = product.Ativo
        };
    }

    public IEnumerable<GetProductDto> Listar()
    {
        return _products.Values
            .OrderBy(prod => prod.Id)
            .Select(prod => new GetProductDto
            {
                Id = prod.Id,
                Nome = prod.Nome,
                Preco = prod.Preco,
                Estoque = prod.Estoque,
                Ativo = prod.Ativo
            })
            .ToList();
    }

    public GetProductDto BuscarPorId(int id)
    {
        if (!_products.TryGetValue(id, out var product))
        {
            throw new ProductNotFoundException(id);
        }

        return new GetProductDto
        {
            Id = product.Id,
            Nome = product.Nome,
            Preco = product.Preco,
            Estoque = product.Estoque,
            Ativo = product.Ativo
        };
    }

    public IEnumerable<GetProductDto> BuscarPorNome(string nome)
    {
        return _products.Values
            .Where(prod => prod.Nome.Contains(nome ?? "", StringComparison.OrdinalIgnoreCase))
            .OrderBy(prod => prod.Id)
            .Select(prod => new GetProductDto
            {
                Id = prod.Id,
                Nome = prod.Nome,
                Preco = prod.Preco,
                Estoque = prod.Estoque,
                Ativo = prod.Ativo
            })
            .ToList();
    }

    public GetProductDto Atualizar(int id, UpdateProductDto dto)
    {
        if (!_products.TryGetValue(id, out var product))
        {
            throw new ProductNotFoundException(id);
        }

        product.Nome = dto.Nome;
        product.Descricao = dto.Descricao;
        product.Estoque = dto.Estoque;
        product.Ativo = dto.Ativo;
        product.Preco = dto.Preco; 

        return new GetProductDto
        {
            Id = product.Id,
            Nome = product.Nome,
            Preco = product.Preco,
            Estoque = product.Estoque,
            Ativo = product.Ativo
        };
    }

    public void Deletar(int id)
    {
        if (!_products.TryRemove(id, out _))
        {
            throw new ProductNotFoundException(id);
        }
    }
}