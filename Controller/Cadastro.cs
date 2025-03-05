namespace CadastroBio.Controller;
using CadastroBio.Model;

public partial class Cadastro: ContentPage
    {
        DataBaseConnection db;
        public Cadastro()
        {
            InitializeComponent();
            db = new DataBaseConnection();
        }

        private async void btn_voltarParaLogin_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//login");
        }

    private async void btn_cadastrarFuncionario_Clicked(object sender, EventArgs e)
    {
        string nome = nomeFuncionario.Text;
        string cpf = cpfFuncionario.Text;
        string email = emailFuncionario.Text;
        string telefone = TelefoneFuncionario.Text;
        string firstSenha = senhaFuncionarioCAD.Text;
        string seconfSenha = senhaUsuarioFuncionarioCAD2.Text;

        if (string.IsNullOrWhiteSpace(nome) ||
            string.IsNullOrWhiteSpace(cpf) ||
            string.IsNullOrWhiteSpace(email)||
            string.IsNullOrWhiteSpace(firstSenha)||
            string.IsNullOrWhiteSpace(seconfSenha))
        {
            await DisplayAlert("Falha", "Favor preencher todos os campos.", "OK");
            return;
        }
        if (!string.Equals(firstSenha, seconfSenha, StringComparison.OrdinalIgnoreCase))
        {
            await DisplayAlert("Falha", "Confirmar senha falhou", "OK");
            return;
        }
        if(cpf.Length != 11)
        {
            await DisplayAlert("Ops", "CPF está errado", "OK");
            return;
        }
        Funcionario funcionario = new Funcionario(nome, cpf, email, telefone, firstSenha);
        try
        {
            bool sucesso = db.CadastrarFuncionario(funcionario);
            if(sucesso = true)
            {
                await DisplayAlert("Sucesso", "Usuário cadastrado com sucesso!", "OK");
                await Shell.Current.GoToAsync("//login");
            }
            else
            {
                await DisplayAlert("Erro", "Falha ao tentar cadastrar usuário", "Tentar novamente");
                return;
            }
        }
        catch
        {
            await DisplayAlert("Erro", "Erro ao realizar a operação", "OK");
            return;
        }
        
    }

    }

