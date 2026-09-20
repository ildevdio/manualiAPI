namespace ManualiAPI.Models;

// Classe que representa a tabela/entidade "Produto" do banco de dados.
// Cada propriedade vira uma coluna (quando se usa EF Core ou similar).
public class Product
{
    // Chave primária, normalmente auto-incrementada pelo banco.
    public int Id { get; set; }

    // "?" significa que a propriedade pode ser null.
    public string? Name { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; } = true;
}