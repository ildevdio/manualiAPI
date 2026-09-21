
namespace ManualiAPI.DTO;

public class CreateAdmDto
{
    public required string Username { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }
}

public class UpdateAdmDto
{
    public int IdAdm { get; set; }

    public required string Username { get; set; }

    public required string Email { get; set; }

    public string? Password { get; set; }
}

public class GetAdmDto
{
    public int IdAdm { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}

public class DeleteAdmDto
{
    public int IdAdm { get; set; }
}