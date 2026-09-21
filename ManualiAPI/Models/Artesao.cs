namespace ManualiAPI.Models;

public class Artesao : User
{
    private string _cpf = string.Empty;

    public string Cpf
    {
        get { return _cpf; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("CPF não pode estar vazio");
            }

            _cpf = value;
        }
    }

    public Artesao(
        string username,
        string password,
        string email,
        string cep,
        string cpf)
        : base(username, password, email, cep)
    {
        Cpf = cpf;
    }
}