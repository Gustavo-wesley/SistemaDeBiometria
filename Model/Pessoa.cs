
namespace CadastroBio.Model;
public class Pessoa
{
    public string nome { get; set;}
    public string cpf { get; set;}

    public Pessoa(string Nome, string CPF)
    {
        nome = Nome;
        cpf = CPF;
    }
}