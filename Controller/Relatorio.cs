namespace CadastroBio.Controller;

public partial class Relatorio : ContentPage
{
    public Relatorio()
    {
        InitializeComponent();
        InfoAluno aluno = new InfoAluno();
        
    }

    private async void btn_cadastrar_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastrarAluno");
    }

    private async void btn_info_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//informacaoAluno");
    }

    private async void btn_sair_Clicked(object sender, EventArgs e)
    {
        bool resposta = await DisplayAlert("Confirmação", "Tem certeza de que deseja sair?", "Sim", "Não");

        if(resposta)
        {
            await Shell.Current.GoToAsync("//login");
        }
    }
}