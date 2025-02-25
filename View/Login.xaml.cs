using System.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace CadastroBio.View;
public partial class Login: ContentPage
{
    public Login()
    {
        InitializeComponent();
    }
    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        senhaUsuario.IsPassword = !senhaUsuario.IsPassword;
    }

    private async void btn_Cadastrar(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastro");
    }

    private void btn_login_Clicked(object sender, EventArgs e)
    {

    }


}
