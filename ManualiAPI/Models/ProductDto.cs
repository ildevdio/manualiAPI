namespace ManualiAPI.Models;

// DTO (Data Transfer Object): serve para controlar o que entra/sai da API.
// Evita expor o modelo do banco diretamente ("? " = opcional/null).
public class CreateProductDto
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateProductDto
{
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}

public class ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}