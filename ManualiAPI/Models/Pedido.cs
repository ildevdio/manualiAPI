namespace ManualiAPI.Models;

public class Pedido
{
    private static int _idPedidoCount = 0;

    private int _idPedido;
    private int _idProduto;
    private DateTime _data;
    private string _cepPedido;
    private bool _concluido;

    public int IdPedido
    {
        get { return _idPedido; }
        set { _idPedido = value; }
    }

    public int IdProduto
    {
        get { return _idProduto; }
        set { _idProduto = value; }
    }

    public DateTime Data
    {
        get { return _data; }
        set { _data = value; }
    }

    public string CepPedido
    {
        get { return _cepPedido; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("CEP não pode estar vazio");
            }

            _cepPedido = value;
        }
    }

    public bool Concluido
    {
        get { return _concluido; }
        set { _concluido = value; }
    }

    public Pedido(int idProduto, DateTime data, string cepPedido)
    {
        _idPedido = _idPedidoCount++;
        IdProduto = idProduto;
        Data = data;
        CepPedido = cepPedido;
        Concluido = false;
    }
}