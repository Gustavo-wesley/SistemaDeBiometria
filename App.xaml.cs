using CadastroBio.Model;

namespace CadastroBio;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        
        // Testar conexão com o banco ao iniciar
        DataBaseConnection db = new DataBaseConnection();
        if (db.TestarConexao())
        {
            Console.WriteLine("✅ Banco de dados conectado com sucesso!");
        }
        else
        {
            Console.WriteLine("❌ Falha na conexão com o banco de dados.");
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
