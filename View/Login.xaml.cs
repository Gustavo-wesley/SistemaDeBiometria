namespace CadastroBio.View;

public partial class Login : ContentPage
{

	public Login()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
		senhaUsuario.IsPassword = !senhaUsuario.IsPassword;
    }

}

