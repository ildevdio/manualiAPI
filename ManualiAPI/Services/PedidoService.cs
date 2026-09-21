using System.Collections.Concurrent;
using ManualiAPI.DTO;
using ManualiAPI.Exceptions;
using ManualiAPI.Models;

namespace ManualiAPI.Services;

public class PedidoService : IPedidoService
{
    private readonly ConcurrentDictionary<int, Pedido> _pedidos = new();
    private readonly IProductService _produtos;
    // Serializa Atualizar x Deletar para o mesmo pedido: a decisão de devolver
    // o estoque (baseada em Concluido) não pode competir com uma atualização.
    private readonly object _pedidoLock = new();

    public PedidoService(IProductService produtos)
    {
        _produtos = produtos;
    }

    public GetPedidoDto Criar(CreatePedidoDto dto)
    {
        // Validação ANTES de reservar: pedido inválido não deve mexer no estoque
        if (dto.Itens is null || dto.Itens.Count == 0)
        {
            throw new ArgumentException("O pedido precisa ter pelo menos um item.");
        }

        if (string.IsNullOrWhiteSpace(dto.Cep))
        {
            throw new ArgumentException("O CEP do pedido não pode estar vazio.");
        }

        // Junta itens repetidos: {prod 1, qtd 2} + {prod 1, qtd 3} -> {prod 1, qtd 5}
        var agrupados = dto.Itens
            .GroupBy(i => i.IdProduto)
            .Select(g => (IdProduto: g.Key, Quantidade: g.Sum(i => i.Quantidade)))
            .ToList();

        // Reserva ESTOQUE e PREÇO de forma atômica (tudo ou nada):
        // valida existência/ativo/estoque e reduz, devolvendo o preço do momento.
        var reservados = _produtos.ReservarEstoque(agrupados);

        try
        {
            var itens = reservados
                .Select(r => new ItemPedido(r.IdProduto, r.Quantidade, r.PrecoUnitario))
                .ToList();

            var pedido = new Pedido(itens, DateTime.UtcNow, dto.Cep);

            _pedidos[pedido.IdPedido] = pedido;
            return ToGetDto(pedido);
        }
        catch
        {
            // Compensação (rollback): se falhar ao montar/salvar o pedido,
            // devolve o estoque que foi reservado e propaga o erro.
            _produtos.DevolverEstoque(
                reservados.Select(r => (r.IdProduto, r.Quantidade)));
            throw;
        }
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

        // Lock: impede que Deletar decida devolver estoque com base num
        // Concluido que está sendo alterado aqui ao mesmo tempo.
        lock (_pedidoLock)
        {
            pedido.CepPedido = dto.Cep;
            pedido.Concluido = dto.Concluido;
        }

        return ToGetDto(pedido);
    }

    public void Deletar(int id)
    {
        if (!_pedidos.TryRemove(id, out var pedido))
        {
            throw new PedidoNotFoundException(id);
        }

        // Pedido ainda não concluído: os produtos voltam para o estoque.
        // Lock: a decisão lê Concluido, que poderia estar sendo mudado por
        // um Atualizar concorrente.
        lock (_pedidoLock)
        {
            if (!pedido.Concluido)
            {
                _produtos.DevolverEstoque(
                    pedido.Itens.Select(i => (i.IdProduto, i.Quantidade)));
            }
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