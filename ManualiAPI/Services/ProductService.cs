using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;

namespace ManualiAPI.Services;

public class ProductService : IProductService
{
    private readonly ConcurrentDictionary<int, Product> _products = new();
    private readonly object _estoqueLock = new();   // novo

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

        lock (_estoqueLock)   // mesmo lock: não altera o estoque no meio de uma redução
        {
            product.Nome = dto.Nome;
            product.Descricao = dto.Descricao;
            product.Estoque = dto.Estoque;
            product.Ativo = dto.Ativo;
            product.Preco = dto.Preco;   // por último: preço negativo desativa o produto
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

    public void Deletar(int id)
    {
        if (!_products.TryRemove(id, out _))
        {
            throw new ProductNotFoundException(id);
        }
    }
    public void ReduzirEstoque(IEnumerable<(int IdProduto, int Quantidade)> itens)
    {
        // Junta ids repetidos para checar o total real de cada produto
        var pedidos = itens
            .GroupBy(i => i.IdProduto)
            .Select(g => (IdProduto: g.Key, Quantidade: g.Sum(i => i.Quantidade)))
            .ToList();

        if (pedidos.Any(p => p.Quantidade <= 0))
        {
            throw new ArgumentException("Quantidade deve ser maior que zero.");
        }

        lock (_estoqueLock)
        {
            // Fase 1: valida TODOS, sem alterar nada
            var alvos = new List<(Product Produto, int Quantidade)>();
            foreach (var (id, quantidade) in pedidos)
            {
                if (!_products.TryGetValue(id, out var produto))
                {
                    throw new ProductNotFoundException(id);
                }

                if (produto.Estoque < quantidade)
                {
                    throw new EstoqueInsuficienteException(produto.Nome, produto.Estoque);
                }

                alvos.Add((produto, quantidade));
            }

            // Fase 2: só agora reduz
            foreach (var (produto, quantidade) in alvos)
            {
                produto.Estoque -= quantidade;
            }
        }
    }

    public void DevolverEstoque(IEnumerable<(int IdProduto, int Quantidade)> itens)
    {
        lock (_estoqueLock)
        {
            foreach (var (id, quantidade) in itens)
            {
                // Se o produto foi deletado nesse meio tempo, não há o que devolver
                if (_products.TryGetValue(id, out var produto))
                {
                    produto.Estoque += quantidade;
                }
            }
        }
    }
}