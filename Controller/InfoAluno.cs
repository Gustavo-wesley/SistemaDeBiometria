namespace CadastroBio.Controller;

public partial class InfoAluno : ContentPage
{
    public InfoAluno()
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

    private async void btn_historico_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//historico");
    }
    private void btn_sair_Clicked(object sender, EventArgs e)
    {
    }
}