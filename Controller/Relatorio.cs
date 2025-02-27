namespace CadastroBio.Controller;

public partial class Relatorio : ContentPage
{
    public Relatorio()
    {
        InitializeComponent();
    }

    private async void btn_cadastrar_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastrarAluno");
    }

    private async void btn_info_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//informacaoAluno");
    }
}