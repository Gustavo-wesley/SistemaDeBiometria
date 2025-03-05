namespace CadastroBio.Model;

public class Funcionario : Pessoa
{
    public int idPessoa;
    public string email { get; set;}
    public string telefone { get; set;}
    public string senha { get; set;}

    public Funcionario(string nome, string cpf, string Email, string Telefone, string Senha)
     : base(nome, cpf)
    {
        email = Email;
        telefone = Telefone;
        senha = Senha;
    }

}