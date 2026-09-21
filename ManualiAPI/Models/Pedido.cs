namespace ManualiAPI.Models;

public class Pedido
{
    private static int _idPedidoCount = 0;

    private int _idPedido;
    private DateTime _data;
    private string _cepPedido = string.Empty;
    private bool _concluido;
    private readonly List<ItemPedido> _itens;

    public int IdPedido
    {
        get { return _idPedido; }
        set { _idPedido = value; }
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

    public IReadOnlyList<ItemPedido> Itens => _itens;

    public decimal Total => _itens.Sum(i => i.Subtotal);

    public Pedido(IEnumerable<ItemPedido> itens, DateTime data, string cepPedido)
    {
        var lista = itens.ToList();

        if (lista.Count == 0)
        {
            throw new ArgumentException("O pedido precisa ter pelo menos um item.");
        }

        _idPedido = Interlocked.Increment(ref _idPedidoCount);
        _itens = lista;
        Data = data;
        CepPedido = cepPedido;
        Concluido = false;
    }
}