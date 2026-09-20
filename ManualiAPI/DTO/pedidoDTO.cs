
namespace ManualiAPI.DTO;

public class CreatePedidoDto
{
    public int IdProduto { get; set; }

    public DateTime Data { get; set; }

    public required string Endereco { get; set; }
}

public class UpdatePedidoDto
{
    public int IdPedido { get; set; }

    public int IdProduto { get; set; }

    public DateTime Data { get; set; }

    public required string Endereco { get; set; }

    public bool Concluido { get; set; }
}

public class GetPedidoDto
{
    public int IdPedido { get; set; }

    public int IdProduto { get; set; }

    public DateTime Data { get; set; }

    public string Endereco { get; set; } = string.Empty;

    public bool Concluido { get; set; }
}

public class DeletePedidoDto
{
    public int IdPedido { get; set; }
}