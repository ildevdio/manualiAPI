namespace ManualiAPI.DTO;

public class CreateItemPedidoDto
{
    public int IdProduto { get; set; }
    public int Quantidade { get; set; }
}

public class CreatePedidoDto
{
    public required List<CreateItemPedidoDto> Itens { get; set; }
    public required string Cep { get; set; }
}

public class UpdatePedidoDto
{
    public required string Cep { get; set; }
    public bool Concluido { get; set; }
}

public class GetItemPedidoDto
{
    public int IdProduto { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}

public class GetPedidoDto
{
    public int IdPedido { get; set; }
    public DateTime Data { get; set; }
    public string Cep { get; set; } = string.Empty;
    public bool Concluido { get; set; }
    public List<GetItemPedidoDto> Itens { get; set; } = new();
    public decimal Total { get; set; }
}