using System;
using System.Data;
using System.Data.SqlClient;
using Model;

namespace Controller
{
    public class AgendamentoController
    {
        ConexaoBancoDadosController conexaoBanco = new ConexaoBancoDadosController();

        public int Cadastrar(Agendamento agendamento)
        {
            try
            {
                string query = "INSERT INTO TBAGENDAMENTO (codigo_prontuario,nome_tmp,telcelular_tmp,telfixo_tmp,data_nascimento,data_agendamento,hora,consultorio,observacoes,data_registro,tipo)VALUES(@codigo_prontuario,@nome_tmp,@telcelular_tmp,@telfixo_tmp,@data_nascimento,@data_agendamento,@hora,@consultorio,@observacoes,@data_registro,@tipo)";

                SqlCommand command = new SqlCommand(query, conexaoBanco.Conectar());
                command.Parameters.AddWithValue("@codigo_prontuario", agendamento.Prontuario.numero);
                command.Parameters.AddWithValue("@nome_tmp", agendamento.Prontuario.nome);
                command.Parameters.AddWithValue("@telcelular_tmp", agendamento.Prontuario.telefoneCelular);
                command.Parameters.AddWithValue("@telfixo_tmp", agendamento.Prontuario.telefoneFixo);
                command.Parameters.AddWithValue("@data_nascimento", agendamento.Prontuario.dataNascimento);
                command.Parameters.AddWithValue("@data_agendamento", agendamento.data_agendamento);
                command.Parameters.AddWithValue("@hora", agendamento.hora);
                command.Parameters.AddWithValue("@consultorio", agendamento.consultorio);
                command.Parameters.AddWithValue("@observacoes", agendamento.observacoes);
                command.Parameters.AddWithValue("@data_registro", DateTime.Today.ToShortDateString());
                command.Parameters.AddWithValue("@tipo", agendamento.tipo);

                return Convert.ToInt32(command.ExecuteNonQuery());
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                conexaoBanco.Fechar();
            }

        }

        public DataTable Pesquisar(string nome, DateTime de, DateTime ate)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM TBAGENDAMENTO WHERE nome_tmp LIKE '%" + nome + "%' AND data_agendamento BETWEEN '" + de.ToShortDateString() + "' AND '" + ate.ToShortDateString() + "' ORDER BY hora", conexaoBanco.Conectar());
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                conexaoBanco.Fechar();
            }
        }

        public int Alterar(Agendamento agendamento)
        {
            try
            {
                string query = "UPDATE TBAGENDAMENTO SET codigo_prontuario = @codigo_prontuario, nome_tmp = @nome_tmp, telcelular_tmp = @telcelular_tmp, telfixo_tmp = @telfixo_tmp, data_nascimento = @data_nascimento, data_agendamento = @data_agendamento, hora = @hora, consultorio = @consultorio, observacoes = @observacoes, data_registro = @data_registro, tipo = @tipo WHERE codigo = @codigo";

                SqlCommand command = new SqlCommand(query, conexaoBanco.Conectar());
                command.Parameters.AddWithValue("@codigo", agendamento.codigo);
                command.Parameters.AddWithValue("@codigo_prontuario", agendamento.Prontuario.numero);
                command.Parameters.AddWithValue("@nome_tmp", agendamento.Prontuario.nome);
                command.Parameters.AddWithValue("@telcelular_tmp", agendamento.Prontuario.telefoneCelular);
                command.Parameters.AddWithValue("@telfixo_tmp", agendamento.Prontuario.telefoneFixo);
                command.Parameters.AddWithValue("@data_nascimento", agendamento.Prontuario.dataNascimento);
                command.Parameters.AddWithValue("@data_agendamento", agendamento.data_agendamento);
                command.Parameters.AddWithValue("@hora", agendamento.hora);
                command.Parameters.AddWithValue("@consultorio", agendamento.consultorio);
                command.Parameters.AddWithValue("@observacoes", agendamento.observacoes);
                command.Parameters.AddWithValue("@data_registro", DateTime.Today.ToShortDateString());
                command.Parameters.AddWithValue("@tipo", agendamento.tipo);

                return Convert.ToInt32(command.ExecuteNonQuery());
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                conexaoBanco.Fechar();
            }

        }

        public int Excluir(int codigo)
        {
            try
            {
                string query = "DELETE FROM TBAGENDAMENTO WHERE codigo = @codigo";

                SqlCommand command = new SqlCommand(query, conexaoBanco.Conectar());
                command.Parameters.AddWithValue("@codigo", codigo);                

                return Convert.ToInt32(command.ExecuteNonQuery());
            }
            catch (Exception erro)
            {
                throw erro;
            }
            finally
            {
                conexaoBanco.Fechar();
            }

        }

        public int MudarStatus(int codigo, string status)
        {
            try
            {
                string query = "UPDATE TBAGENDAMENTO SET status = @status WHERE codigo = @codigo";

                SqlCommand command = new SqlCommand(query, conexaoBanco.Conectar());
                command.Parameters.AddWithValue("@codigo", codigo);
                command.Parameters.AddWithValue("@status", status);

                return Convert.ToInt32(command.ExecuteNonQuery());
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                conexaoBanco.Fechar();
            }

        }
    }
}
