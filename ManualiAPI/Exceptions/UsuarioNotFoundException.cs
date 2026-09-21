namespace ManualiAPI.Exceptions;

public class UsuarioNotFoundException : Exception
{
    public UsuarioNotFoundException(int id)
    : base($"Usuário de Id {id} não existe ou não foi localizado...")
    {
    }
    
    public UsuarioNotFoundException(string username)
    : base($"Usuário de nome {username} não existe ou não foi localizado...")
    {
    }
}