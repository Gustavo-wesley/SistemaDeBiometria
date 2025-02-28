namespace CadastroBio.Model;
using Npgsql;
public class DataBaseConnection
{
    private string conexao = "Host=localhost;Port=5432;Username=postgres;Password=4321;Database=BDbio;";

    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(conexao);
    }

    public bool TestarConexao()
    {
        try
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                Console.WriteLine("Conexao bem-sucedida com o banco de dados!");
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao conectar: " + ex.Message);
            return false;
        }
    }

    public Funcionario VerificarFuncionario(string email, string senha){

        using (var connection = new NpgsqlConnection(conexao))
        {
            connection.Open();
            string query = "SELECT pessoa.nome, pessoa.cpf, funcionario.email, funcionario.telefone, funcionario.senha From funcionario JOIN pessoa ON funcionario.idFuncionario = pessoa.idPessoa WHERE email = @email AND senha = @senha";

            using (var cmd = new NpgsqlCommand(query, connection)){
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@senha", senha);

                using (var reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        return new Funcionario
                        (
                            reader.GetString(0),  // Nome
                            reader.GetString(1),  // CPF
                            reader.GetString(2),  // Email
                            reader.GetString(3),  // Telefone
                            reader.GetString(4)   // Senha
                        );
                    }
                }
            }
        }

        return null;

    }

    public bool CadastrarFuncionario(Funcionario funcionario)
    {
        using (var connection = new NpgsqlConnection(conexao))
        {
            connection.Open();

            string queryPessoa = 
            "INSERT INTO pessoa(nome,cpf) Values (@nome, @cpf) RETURNING idPessoa";
            string queryFuncionario = 
            "INSERT INTO funcionario(idFuncionario, email, telefone, senha) VALUES (@idFuncionario, @email, @telefone, @senha)";

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var cmdPessoa = new NpgsqlCommand(queryPessoa, connection, transaction))
                    {
                        cmdPessoa.Parameters.AddWithValue("@nome", funcionario.nome);
                        cmdPessoa.Parameters.AddWithValue("@cpf", funcionario.cpf);
                        int idPessoa = (int)cmdPessoa.ExecuteScalar();

                        using (var cmdFuncionario = new NpgsqlCommand(queryFuncionario, connection, transaction))
                        {
                            cmdFuncionario.Parameters.AddWithValue("@idFuncionario", idPessoa);
                            cmdFuncionario.Parameters.AddWithValue("@email", funcionario.email);
                            cmdFuncionario.Parameters.AddWithValue("@telefone", funcionario.telefone);
                            cmdFuncionario.Parameters.AddWithValue("@senha", funcionario.senha);
                            cmdFuncionario.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao tentar cadastrar funcionario", ex.Message);
                    return false;
                }
            }
        }
    }

    public bool cadastrarAluno(Aluno aluno, CadastroBiometrico cadastroBiometria)
    {
        using (var connection = new NpgsqlConnection(conexao))
        {
            connection.Open();

            string queryPessoa = 
            "INSERT INTO pessoa(nome,cpf) Values (@nome, @cpf) RETURNING idPessoa";
            string queryAluno = 
            "INSERT INTO aluno(idAluno, matricula, curso, auxilio) VALUES (@idAluno, @matricula, @curso, @auxilio)";
            string queryBiometria =
            "INSERT INTO cadastroBiometrico(idAluno, templateBiometrico, dataCadastro) VALUES (@idAlunok ,@templateBiometrico, @dataCadastro) RETURNING idBiometria";

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var cmdPessoa = new NpgsqlCommand(queryPessoa, connection, transaction))
                    {
                        cmdPessoa.Parameters.AddWithValue("@nome", aluno.nome);
                        cmdPessoa.Parameters.AddWithValue("@cpf", aluno.cpf);
                        int idPessoa = (int)cmdPessoa.ExecuteScalar();

                        using (var cmdAluno = new NpgsqlCommand(queryAluno, connection, transaction))
                        {
                            cmdAluno.Parameters.AddWithValue("@idAluno", idPessoa);
                            cmdAluno.Parameters.AddWithValue("@matricula", aluno.matricula);
                            cmdAluno.Parameters.AddWithValue("@curso", aluno.curso);
                            cmdAluno.Parameters.AddWithValue("@auxilio", aluno.auxilio);
                            cmdAluno.ExecuteNonQuery();
                            
                            using (var cmdCadastroBiometrico = new NpgsqlCommand(queryBiometria, connection, transaction))
                            {
                                cmdCadastroBiometrico.Parameters.AddWithValue("@idAlunok", idPessoa);
                                cmdCadastroBiometrico.Parameters.AddWithValue("@templateBiometrico",NpgsqlTypes.NpgsqlDbType.Bytea, cadastroBiometria.templateBiometrico);
                                cmdCadastroBiometrico.Parameters.AddWithValue("@dataCadastro", cadastroBiometria.dataCadastro);
                                cmdCadastroBiometrico.ExecuteNonQuery();
                            }
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao tentar cadastrar aluno", ex.Message);
                    return false;
                }
            }
        }
    }
}