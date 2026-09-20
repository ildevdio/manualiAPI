namespace ManualiAPI.Exceptions;

public class PedidoNotFoundException : Exception
{
    public PedidoNotFoundException(int id)
    : base($"Pedido com Id {id} não foi encontrado... ")
    {
    }
}