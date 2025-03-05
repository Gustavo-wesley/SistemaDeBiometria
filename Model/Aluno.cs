namespace CadastroBio.Model;

public class Aluno : Pessoa
{
    public int idPessoa;
    public string matricula { get; set;}
    public string curso { get; set;}
    public bool auxilio { get; set;}

    public Aluno(string nome, string cpf,string Matricula, string Curso, bool Auxilio) : base (nome, cpf)
    {
        matricula = Matricula;
        curso = Curso;
        auxilio = Auxilio;
    }
}