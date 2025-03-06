using CadastroBio.Model;

namespace CadastroBio.Controller;
public partial class Login: ContentPage
{
    private DataBaseConnection db;
    public Login()
    {
        InitializeComponent();
        db = new DataBaseConnection();
    }

    private void cleanView()
    {
        EmailUsuario.Text = "";
        senhaUsuario.Text = "";
    }
    
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        senhaUsuario.IsPassword = !senhaUsuario.IsPassword;
        
    }

    private async void btn_Cadastrar(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastro");
    }

    private async void btn_login_Clicked(object sender, EventArgs e)
    {
        string email = EmailUsuario.Text;
        string senha = senhaUsuario.Text;

        if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        {
            await DisplayAlert("Falha", "Preencha todos os campos", "OK");
            return;
        }
        try
        {
            Funcionario funcionario = db.VerificarFuncionario(email, senha);

            if(funcionario != null){
                await DisplayAlert("Sucesso", "Login efetuado com sucesso", "OK");
                await Shell.Current.GoToAsync("//cadastrarAluno");
            }
            else
            {
                await DisplayAlert("Ops", "E-mail ou Senha incorretos!", "Tentar novamente");
                return;
            }
        }
        catch(Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
            return;
        }
    }


}
