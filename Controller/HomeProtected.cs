namespace CadastroBio.Controller;

public partial class HomeProtected: ContentPage
{
    public HomeProtected()
    {
        InitializeComponent();
    }

    private async void btn_cadastrar_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastrarAluno");
    }

    private async void Alterar_info_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//informacaoAluno");
    }

    private async void Gerar_relatorio_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//relatorio");
    }

    private async void Consultar_Histórico_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//historico");
    }

}