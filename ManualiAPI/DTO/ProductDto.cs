namespace ManualiAPI.DTO;

public class CreateProductDto
{
    public required string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public decimal Ativo { get; set; }
}

public class UpdateProductDto
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public decimal Ativo { get; set; }
}

public class DeleteProductDto
{
    public required string Nome { get; set; }
}

public class GetProductDto
{
    public string? Nome { get; set; }
    public decimal? Preco { get; set; }
}