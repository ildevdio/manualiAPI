namespace ManualiAPI.DTO;

public class CreateProductDto
{
    public required string Nome { get; set; }
    public string Descricao { get; set; } = "";
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; } = true;
}

public class UpdateProductDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public string Descricao { get; set; } = "";
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
}

public class DeleteProductDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
}

public class GetProductDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = "";
    public decimal Preco { get; set; }
    public bool Ativo { get; set; }
    public int Estoque { get; set; }
}