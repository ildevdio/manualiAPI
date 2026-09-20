namespace ManualiAPI.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(int id)
        : base($"Produto com Id {id} não foi encontrado.")
    {
    }
}