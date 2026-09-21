namespace ManualiAPI.Models;

public abstract class User
{
    private static int _idCount = 0;

    private int _id;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _email = string.Empty;
    private string _cep = string.Empty;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public string Username
    {
        get { return _username; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Nome não pode estar vazio");
            }

            _username = value;
        }
    }

    public string Password
    {
        get { return _password; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Senha não pode estar vazia");
            }

            _password = value;
        }
    }

    public string Email
    {
        get { return _email; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("E-mail não pode estar vazio");
            }

            _email = value;
        }
    }

    public string Cep
    {
        get { return _cep; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("CEP não pode estar vazio");
            }

            _cep = value;
        }
    }

    public User(string username, string password, string email, string cep)
    {
        _id = Interlocked.Increment(ref _idCount);

        Username = username;
        Password = password;
        Email = email;
        Cep = cep;
    }
}