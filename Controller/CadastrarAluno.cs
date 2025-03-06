namespace CadastroBio.Controller;
using CadastroBio.Model;

public partial class CadastrarAluno: ContentPage
{
    bool concluido = false;
    bool bioButtonClicked = false;
    DataBaseConnection db;
    public CadastrarAluno()
    {
        InitializeComponent();
        db = new DataBaseConnection();
    }

    private async void btn_info_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//informacaoAluno");
    }

    private async void btn_gerar_relatorio_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//relatorio");
    }

    private void btn_gerarBiometria(object sender, EventArgs e)
    {
        gerouBiometria.IsVisible = true;
        bioButtonClicked = true;
    }

    private void cleanView()
    {
        if(concluido)
        {
            nomeAluno.Text = "";
            cpfAluno.Text = "";
            matriculaAluno.Text = "";
            cursoAluno.Text = "";
            auxilioAluno.SelectedItem = null;
            gerouBiometria.IsVisible = false;

        }
    }
    private async void btn_inserir_aluno_Clicked(object sender, EventArgs e)
    {
        string nome = nomeAluno.Text;
        string cpf = cpfAluno.Text;
        string matricula = matriculaAluno.Text;
        string curso = cursoAluno.Text;
        string auxilioSelecionado = auxilioAluno.SelectedItem?.ToString();

        DateTime dataAtual = DateTime.Now;

        byte[] biometria = new byte[16]; // Tamanho de 16 bytes (pode ser ajustado)
        Random random = new Random();
        random.NextBytes(biometria); // Preenche o array com valores aleatórios

        if(string.IsNullOrWhiteSpace(nome) ||
        string.IsNullOrWhiteSpace(cpf) ||
        string.IsNullOrWhiteSpace(matricula)||
        string.IsNullOrWhiteSpace(curso)||
        string.IsNullOrWhiteSpace(auxilioSelecionado))
        {
            await DisplayAlert("Falha", "Favor preencher todos os campos.", "OK");
            return;
        }

        if(cpf.Length != 11)
        {
            await DisplayAlert("Ops", "CPF está errado", "OK");
            return;
        }

        if(!bioButtonClicked)
        {
            await DisplayAlert("Erro", "Favor gerar a biometria", "Tentar novamente");
            return;
        }

        bool auxilioEstudantil = auxilioSelecionado == "true";

        Aluno aluno = new Aluno (nome, cpf, matricula, curso, auxilioEstudantil);
        CadastroBiometrico cadastroBiometrico = new CadastroBiometrico (biometria, dataAtual);

        try
        {
            bool sucesso = db.cadastrarAluno(aluno, cadastroBiometrico);
            if(sucesso)
            {
                await DisplayAlert("Sucesso", "Aluno cadastrado com sucesso!", "OK");
                concluido = true;
                cleanView();
            }
            else
            {
                await DisplayAlert("Erro", "Falha ao tentar cadastrar aluno", "Tentar novamente");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao tentar realizar a operação", ex.Message);
            return;
        }

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