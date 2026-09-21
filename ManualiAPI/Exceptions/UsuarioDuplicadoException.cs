namespace ManualiAPI.Exceptions;

public class UsuarioDuplicadoException : Exception
{
    public UsuarioDuplicadoException()
    {
    }

    public UsuarioDuplicadoException(string mensagem) : base(mensagem)
    {
    }
}