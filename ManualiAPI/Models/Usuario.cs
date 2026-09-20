namespace ManualiAPI.Models;

public class Usuario : User
{
    public Usuario(string username, string password, string email, string cep)
        : base(username, password, email, cep)
    {
    }
}