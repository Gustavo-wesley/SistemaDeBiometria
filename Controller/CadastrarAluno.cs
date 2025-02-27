namespace CadastroBio.Controller;

public partial class CadastrarAluno: ContentPage
{
    public CadastrarAluno()
    {
        InitializeComponent();
    }

    private async void btn_info_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//informacaoAluno");
    }

    private async void btn_gerar_relatorio_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//relatorio");
    }

    private void btn_inserir_aluno_Clicked(object sender, EventArgs e)
    {
    }

}