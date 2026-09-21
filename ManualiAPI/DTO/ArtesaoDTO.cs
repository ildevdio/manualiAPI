
namespace ManualiAPI.DTO;

public class CreateArtesaoDto
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string Cep { get; set; }

    public required string Cpf { get; set; }
}

public class UpdateArtesaoDto
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public required string Cep { get; set; }

    public required string Cpf { get; set; }

    public string? Password { get; set; }
}

public class GetArtesaoDto
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Cep { get; set; } = string.Empty;
}

public class DeleteArtesaoDto
{
    public int Id { get; set; }
}