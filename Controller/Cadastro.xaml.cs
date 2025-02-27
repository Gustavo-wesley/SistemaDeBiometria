namespace CadastroBio.Controller;

    public partial class Cadastro: ContentPage
    {
        public Cadastro()
        {
            InitializeComponent();
        }

        private async void btn_voltarParaLogin_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//login");
        }

    private void btn_cadastrarFuncionario_Clicked(object sender, EventArgs e)
    {
    }

    }

