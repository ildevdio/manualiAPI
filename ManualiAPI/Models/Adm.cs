namespace ManualiAPI.Models;

public class Adm
{
    private static int _idAdmCount = 0;

    private int _idAdm;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _email = string.Empty;

    public int IdAdm
    {
        get { return _idAdm; }
        set { _idAdm = value; }
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

    public Adm(string username, string password, string email)
    {
        _idAdm = Interlocked.Increment(ref _idAdmCount);

        Username = username;
        Password = password;
        Email = email;
    }
}