namespace ManualiAPI.Models;

public class ItemPedido
{
    public int IdProduto { get; }
    public int Quantidade { get; }
    public decimal PrecoUnitario { get; }   // preço copiado no momento da compra

    public ItemPedido(int idProduto, int quantidade, decimal precoUnitario)
    {
        if (quantidade <= 0)
        {
            throw new ArgumentException("Quantidade deve ser maior que zero.");
        }

        IdProduto = idProduto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}