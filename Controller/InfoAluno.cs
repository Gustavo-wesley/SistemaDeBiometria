namespace CadastroBio.Controller;
using CadastroBio.Model;

public partial class InfoAluno : ContentPage
{
    Aluno aluno;
    DataBaseConnection db;
    public InfoAluno()
    {
        InitializeComponent();
        db = new DataBaseConnection();
    }

    private void cleanView()
    {
        entryNome.Text = "";
        entryCPF.Text = "";
        entryNovaMatricula.Text = "";
        entryCurso.Text = "";
        switchAuxilio.IsToggled = false;
        excluirAluno.IsVisible = false;
    }
    private async void procurarAluno_Clicked(object sender, EventArgs e)
    {
        string Matricula = entryMatricula.Text;

        if(string.IsNullOrWhiteSpace(Matricula))
        {
            await DisplayAlert("Erro", "Favor informar matricula", "OK");
            return;
        }
        
        try
        {
            aluno = db.ProcurarAluno(Matricula);

            if(aluno != null)
            {
                entryNome.Text = aluno.nome;
                entryCPF.Text = aluno.cpf;
                entryNovaMatricula.Text = aluno.matricula;
                entryCurso.Text = aluno.curso;
                switchAuxilio.IsToggled = aluno.auxilio;

                entryNome.IsEnabled = true;
                entryCPF.IsEnabled = true;
                entryNovaMatricula.IsEnabled = true;
                entryCurso.IsEnabled = true;
                switchAuxilio.IsEnabled = true;
                excluirAluno.IsVisible = true;

            }
            else
            {   
                await DisplayAlert("Ops", "Aluno não encontrado", "Tentar novamente");
                aluno = null;
                entryNome.IsEnabled = false;
                entryCPF.IsEnabled = false;
                entryNovaMatricula.IsEnabled = false;
                entryCurso.IsEnabled = false;
                switchAuxilio.IsEnabled = false;
                return;
            }
        }
        catch
        {
            await DisplayAlert("Erro", "Erro ao realizar operação", "Tentar novamente");
            aluno = null;
            return;
        }
    }


    private async void alterarAluno_Clicked(object sender, EventArgs e)
    {
        if(aluno == null)
        {
            await DisplayAlert("Erro", "Aluno não encontrada para salvar alterações", "Tentar novamente");
            return;
        }
        if( entryNome.Text == aluno.nome &&
            entryCPF.Text == aluno.cpf &&
            entryNovaMatricula.Text == aluno.matricula &&
            entryCurso.Text == aluno.curso &&
            switchAuxilio.IsToggled == aluno.auxilio)
        {
            await DisplayAlert("Erro", "Nenhuma alteração foi feita", "Tentar novamente");
            return;
        }
        string matriculaAntiga = aluno.matricula;
        string cpfAntigo = aluno.cpf;

        aluno.nome = entryNome.Text;
        aluno.cpf = entryCPF.Text;
        aluno.matricula = entryNovaMatricula.Text;
        aluno.curso = entryCurso.Text;
        aluno.auxilio = switchAuxilio.IsToggled;

        try
        {
            bool sucesso = db.AlterarAluno(aluno, cpfAntigo, matriculaAntiga);
            if(sucesso)
            {
                await DisplayAlert("Sucesso", "Aluno alterado com sucesso!", "OK");
                cleanView();
                aluno = null;
            }
            else
            {
                await DisplayAlert("Erro", "Erro ao alterar aluno", "OK");
                return;
            }
        }
        catch
        {
            await DisplayAlert("Erro", "Erro ao tentar realizar a operação", "OK");
            return;
        }
    }

    private async void btn_cadastrar_aluno_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//cadastrarAluno");
    }

    private async void btn_gerar_relatorio_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//relatorio");
    }

    private async void btn_sair_Clicked(object sender, EventArgs e)
    {
        bool resposta = await DisplayAlert("Confirmação", "Tem certeza de que deseja sair?", "Sim", "Não");

        if(resposta)
        {
            await Shell.Current.GoToAsync("//login");
        }
    }

    private async void excluirAluno_Clicked(object sender, EventArgs e)
    {
        if(aluno == null)
        {
            await DisplayAlert("Erro", "Nenhuma usuário selecionado", "OK");
            return;
        }

        bool confirmar = await DisplayAlert("Confirmação","Tem certeza que deseja excluir o aluno?", "Sim", "Não");

        if(!confirmar)
        {
            return;
        }

        try
        {
            bool sucesso = db.ExcluirAluno(aluno.matricula, aluno.cpf);

            if(sucesso)
            {
                await DisplayAlert("Sucesso", "Aluno excluído com sucesso!", "OK");
                entryMatricula.Text = "";
                cleanView();
            }
            else
            {
                await DisplayAlert("Erro", "Falha ao tentar excluir aluno", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Erro ao tentar realizar a operação: " + ex.Message, "OK");
            return;
        }
    }

}