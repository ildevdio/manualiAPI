namespace ManualiAPI.Exceptions;

public class EstoqueInsuficienteException : Exception
{
    public EstoqueInsuficienteException(string nomeProduto, int disponivel)
        : base($"Estoque insuficiente para '{nomeProduto}'. Disponível: {disponivel}.")
    {
    }
}