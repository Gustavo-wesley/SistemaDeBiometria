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
            "INSERT INTO aluno(idAluno, matricula, curso, auxilioEstudantil) VALUES (@idAluno, @matricula, @curso, @auxilio)";
            string queryBiometria =
            "INSERT INTO cadastroBiometrico(idAluno, templateBiometrico, dataCadastro) VALUES (@idAluno ,@templateBiometrico, @dataCadastro) RETURNING idBiometria";

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
                                cmdCadastroBiometrico.Parameters.AddWithValue("@idAluno", idPessoa);
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

    public Aluno ProcurarAluno(string matricula)
    {
        using (var connection = new NpgsqlConnection(conexao))
        {
            connection.Open();
            string queryAcharAluno =
            "SELECT pessoa.nome, pessoa.cpf, aluno.matricula, aluno.curso, aluno.auxilioEstudantil FROM pessoa JOIN aluno ON pessoa.idPessoa = aluno.idAluno WHERE aluno.matricula = @matricula";

            using (var cmdMatAluno = new NpgsqlCommand(queryAcharAluno, connection))
            {
                cmdMatAluno.Parameters.AddWithValue("@matricula", matricula);

                using (var reader = cmdMatAluno.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        return new Aluno
                        (
                            reader.GetString(0),  // Nome
                            reader.GetString(1),  // CPF
                            reader.GetString(2),  // matricula
                            reader.GetString(3),  // Curso
                            reader.GetBoolean(4)  // auxilio
                        );
                    }
                }
            }
        }
        return null;
    }

    public bool AlterarAluno(Aluno aluno, string cpfAntigo, string matriculaAntiga)
    {
        using (var connection = new NpgsqlConnection(conexao))
        {
            connection.Open();
            string queryPessoa = 
            "UPDATE pessoa SET nome = @nome, cpf = @cpf WHERE cpf = @cpfAntigo";

            string queryAluno = 
            "UPDATE aluno SET matricula = @matricula, curso = @curso, auxilioEstudantil = @auxilio WHERE matricula = @matriculaAntiga";

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var cmdAlterPessoa = new NpgsqlCommand(queryPessoa, connection, transaction))
                    {
                        cmdAlterPessoa.Parameters.AddWithValue("@cpfAntigo", cpfAntigo);
                        cmdAlterPessoa.Parameters.AddWithValue("@nome", aluno.nome);
                        cmdAlterPessoa.Parameters.AddWithValue("@cpf", aluno.cpf);
                        cmdAlterPessoa.ExecuteNonQuery();
                        Console.WriteLine("Esta indo");

                        using (var cmdAlterAluno = new NpgsqlCommand(queryAluno, connection, transaction))
                        {
                            cmdAlterAluno.Parameters.AddWithValue("@matriculaAntiga", matriculaAntiga);
                            cmdAlterAluno.Parameters.AddWithValue("@matricula", aluno.matricula);
                            cmdAlterAluno.Parameters.AddWithValue("@curso", aluno.curso);
                            cmdAlterAluno.Parameters.AddWithValue("@auxilio", aluno.auxilio);
                            Console.WriteLine("Esta indo");
                            cmdAlterAluno.ExecuteNonQuery();
                            Console.WriteLine("Esta indo");
                        }
                    }
                    Console.WriteLine("Esta indo");
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Erro ao tentar alterar aluno", ex.Message);
                    return false;
                }
            }
        }
    }

    public bool ExcluirAluno(string matricula, string cpf)
    {
        using (var connection = new NpgsqlConnection(conexao))
    {
        connection.Open();

        string queryExcluirBiometria = "DELETE FROM cadastroBiometrico WHERE idAluno = (SELECT idAluno FROM aluno WHERE matricula = @matricula)";
        string queryExcluirAluno = "DELETE FROM aluno WHERE matricula = @matricula";
        string queryExcluirPessoa = "DELETE FROM pessoa WHERE cpf = @cpf";

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                using (var cmdExcluirBiometria = new NpgsqlCommand(queryExcluirBiometria, connection, transaction))
                {
                    cmdExcluirBiometria.Parameters.AddWithValue("@matricula", matricula);
                    cmdExcluirBiometria.ExecuteNonQuery();
                }

                using (var cmdExcluirAluno = new NpgsqlCommand(queryExcluirAluno, connection, transaction))
                {
                    cmdExcluirAluno.Parameters.AddWithValue("@matricula", matricula);
                    cmdExcluirAluno.ExecuteNonQuery();
                }

                using (var cmdExcluirPessoa = new NpgsqlCommand(queryExcluirPessoa, connection, transaction))
                {
                    cmdExcluirPessoa.Parameters.AddWithValue("@cpf", cpf);
                    cmdExcluirPessoa.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Erro ao tentar excluir aluno: " + ex.Message);
                return false;
            }
        }
    }
    }
}