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

}