namespace ManualiAPI.Models;

public class Product
{
    private static int _idCount = 0;
    private int _id;
    private string _nome = string.Empty;
    private string _descricao = string.Empty;
    private decimal _preco;
    private int _estoque = 0;
    private bool _ativo = true;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Nome
    {
        get { return _nome; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Nome não pode estar vazio.");
            }
            _nome = value;
        }
    }

    public string Descricao
    {
        get { return _descricao; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            { 
                _descricao = " ";
            }
            else
            {
                _descricao = value;
            }
        }
    }

    public decimal Preco
    {
        get { return _preco; }
        set
        {
            if (value < 0)
            {
                _preco = 0;
                _ativo = false;
            }
            else
            {
                _preco = value;
            }
        }
    }

    public int Estoque
    {
        get { return _estoque; }
        set { _estoque = value; }
    }

    public bool Ativo
    {
        get { return _ativo; }
        set { _ativo = value; }
    }
    
    public Product(string nome, string descricao, decimal preco, int estoque, bool ativo)
    {
        this._id = Interlocked.Increment(ref _idCount);
        
        Nome = nome;
        Descricao = descricao;
        Estoque = estoque;
        Ativo = ativo;
        Preco = preco;   // por último: preço negativo desativa o produto
    }
}
