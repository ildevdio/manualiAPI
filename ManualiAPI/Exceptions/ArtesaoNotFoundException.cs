namespace ManualiAPI.Exceptions;

public class ArtesaoNotFoundException : Exception
{
    public ArtesaoNotFoundException(int id)
        : base($"Artesão com Id {id} não foi encontrado.") { }

    public ArtesaoNotFoundException(string username)
        : base($"Artesão '{username}' não foi encontrado.") { }
}