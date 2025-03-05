namespace CadastroBio.Controller;
public partial class Historico : ContentPage
{
    public Historico()
    {
        InitializeComponent();
    }

    private async void btn_cadastrar_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastrarAluno");
    }

    private async void btn_gerar_relatorio_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//relatorio");
    }

    private async void btn_info_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//informacaoAluno");
    }

    private void btn_sair_Clicked(object sender, EventArgs e)
    {
    }
}