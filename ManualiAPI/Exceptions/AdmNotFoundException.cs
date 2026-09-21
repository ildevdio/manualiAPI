namespace ManualiAPI.Exceptions;

public class AdmNotFoundException : Exception
{
    public AdmNotFoundException(int id)
        : base($"Administrador com Id {id} não foi encontrado.") { }

    public AdmNotFoundException(string username)
        : base($"Administrador '{username}' não foi encontrado.") { }
}