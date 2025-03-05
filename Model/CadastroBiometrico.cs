namespace CadastroBio.Model;

public class CadastroBiometrico
{
    public int idAluno;
    public byte[] templateBiometrico { get; set;}
    public DateTime dataCadastro { get; set;}

    public CadastroBiometrico(byte[] TemplateBiometrico, DateTime DataCadastro)
    {
        templateBiometrico = TemplateBiometrico;
        dataCadastro = DataCadastro;
    }
}