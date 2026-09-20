namespace ManualiAPI.Models;

public class User
{
    private static int _idCount = 0;
    private int _id;
    private string _username;
    private string _password;
    private string _email;

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
        }
    }

    public User(string username, string password, string email)
    {
        this._id = _idCount++;
        
        this._username = username;
        this._password = password;
        this._email = email;
    }
    
}