using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;

namespace ManualiAPI.Services;

public class PedidoService : IPedidoService
{
    private readonly ConcurrentDictionary<int, Pedido> _pedidos = new();
    private readonly IProductService _produtos;

    public PedidoService(IProductService produtos)
    {
        _produtos = produtos;
    }

    public GetPedidoDto Criar(CreatePedidoDto dto)
    {
        if (dto.Itens is null || dto.Itens.Count == 0)
        {
            throw new ArgumentException("O pedido precisa ter pelo menos um item.");
        }

        // Junta itens repetidos: {prod 1, qtd 2} + {prod 1, qtd 3} -> {prod 1, qtd 5}
        var agrupados = dto.Itens
            .GroupBy(i => i.IdProduto)
            .Select(g => (IdProduto: g.Key, Quantidade: g.Sum(i => i.Quantidade)))
            .ToList();

        // Valida produto (existe? ativo?) e monta os itens com o preço ATUAL
        var itens = new List<ItemPedido>();
        foreach (var (idProduto, quantidade) in agrupados)
        {
            var produto = _produtos.BuscarPorId(idProduto);   // 404 se não existir

            if (!produto.Ativo)
            {
                throw new ArgumentException($"O produto '{produto.Nome}' está inativo.");
            }

            // O ItemPedido valida quantidade > 0
            itens.Add(new ItemPedido(idProduto, quantidade, produto.Preco));
        }

        // Constrói o pedido ANTES de mexer no estoque: se o CEP for vazio,
        // o construtor lança e nada foi reduzido
        var pedido = new Pedido(itens, DateTime.UtcNow, dto.Cep);

        // Estoque: valida e reduz tudo de uma vez (lança 409 se faltar)
        _produtos.ReduzirEstoque(agrupados);

        _pedidos[pedido.IdPedido] = pedido;
        return ToGetDto(pedido);
    }

    public IEnumerable<GetPedidoDto> Listar()
    {
        return _pedidos.Values
            .OrderBy(p => p.IdPedido)
            .Select(ToGetDto)
            .ToList();
    }

    public GetPedidoDto BuscarPorId(int id)
    {
        if (!_pedidos.TryGetValue(id, out var pedido))
        {
            throw new PedidoNotFoundException(id);
        }

        return ToGetDto(pedido);
    }

    public GetPedidoDto Atualizar(int id, UpdatePedidoDto dto)
    {
        if (!_pedidos.TryGetValue(id, out var pedido))
        {
            throw new PedidoNotFoundException(id);
        }

        pedido.CepPedido = dto.Cep;
        pedido.Concluido = dto.Concluido;

        return ToGetDto(pedido);
    }

    public void Deletar(int id)
    {
        if (!_pedidos.TryRemove(id, out var pedido))
        {
            throw new PedidoNotFoundException(id);
        }

        // Pedido ainda não concluído: os produtos voltam para o estoque
        if (!pedido.Concluido)
        {
            _produtos.DevolverEstoque(
                pedido.Itens.Select(i => (i.IdProduto, i.Quantidade)));
        }
    }

    private static GetPedidoDto ToGetDto(Pedido p) => new()
    {
        IdPedido = p.IdPedido,
        Data = p.Data,
        Cep = p.CepPedido,
        Concluido = p.Concluido,
        Total = p.Total,
        Itens = p.Itens.Select(i => new GetItemPedidoDto
        {
            IdProduto = i.IdProduto,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Subtotal = i.Subtotal
        }).ToList()
    };
}