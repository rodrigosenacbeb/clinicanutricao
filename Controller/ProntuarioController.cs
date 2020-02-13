using System;
using System.Data;
using System.Data.SqlClient;
using Model;

namespace Controller
{
    public class ProntuarioController
    {
        ConexaoBancoDadosController conexaoBanco = new ConexaoBancoDadosController();

        /// <summary>
        /// Retorna o código gerado pelo prontuário. Padrão do código NUMERO/ANO.
        /// </summary>
        public int Cadastrar(Prontuario prontuario)
        {
            try
            {
                string query = "INSERT INTO TBPRONTUARIO (numero , nome , sexo, nomemae, nomepai, datanascimento, idade, classificacao, logradouro, bairro, cep, cidade, uf, dataregistro, telefoneFixo, telefoneCelular, origemEncaminhamento, medicoResponsavel, observacao) VALUES(@numero, @nome , @sexo, @nomemae, @nomepai, @datanascimento, @idade, @classificacao, @logradouro, @bairro, @cep, @cidade, @uf, @dataregistro, @telefoneFixo, @telefoneCelular, @origemEncaminhamento, @medicoResponsavel, @observacao)";

                SqlCommand command = new SqlCommand(query, conexaoBanco.Conectar());
                command.Parameters.AddWithValue("@numero", prontuario.numero);
                command.Parameters.AddWithValue("@nome", prontuario.nome);
                command.Parameters.AddWithValue("@sexo", prontuario.sexo);
                command.Parameters.AddWithValue("@nomemae", prontuario.nomeMae);
                command.Parameters.AddWithValue("@nomepai", prontuario.nomePai);
                command.Parameters.AddWithValue("@datanascimento", prontuario.dataNascimento);
                command.Parameters.AddWithValue("@idade", prontuario.idade);
                command.Parameters.AddWithValue("@classificacao", prontuario.classificacao);
                command.Parameters.AddWithValue("@logradouro", prontuario.logradouro);
                command.Parameters.AddWithValue("@bairro", prontuario.bairro);
                command.Parameters.AddWithValue("@cep", prontuario.cep);
                command.Parameters.AddWithValue("@cidade", prontuario.cidade);
                command.Parameters.AddWithValue("@uf", prontuario.uf);
                command.Parameters.AddWithValue("@dataregistro", DateTime.Today.ToShortDateString());
                command.Parameters.AddWithValue("@telefoneFixo", prontuario.telefoneFixo);
                command.Parameters.AddWithValue("@telefoneCelular", prontuario.telefoneCelular);
                command.Parameters.AddWithValue("@origemEncaminhamento", prontuario.origemEncaminhamento);
                command.Parameters.AddWithValue("@medicoResponsavel", prontuario.medicoResponsavel);                
                command.Parameters.AddWithValue("@observacao", prontuario.observacao);

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

        public string UltimoNumeroProntuario()
        {
            try
            {             
                SqlCommand command = new SqlCommand("SELECT TOP 1 numero FROM TBPRONTUARIO ORDER BY numero DESC", conexaoBanco.Conectar());
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);

                if(dt.Rows.Count == 0)
                {
                    return Properties.Settings.Default.numeroProntuario + "/" + DateTime.Today.Year.ToString();
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        string retorno = item["numero"].ToString();
                        string[] separado = retorno.Split('/');
                        return (Convert.ToInt64(separado[0]) + 1).ToString() + "/" + DateTime.Today.Year.ToString();
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                conexaoBanco.Fechar();
            }

            return "";
        }

        public DataTable Pesquisar(string nome)
        {
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM TBPRONTUARIO WHERE nome LIKE '%" + nome + "%' ORDER BY nome", conexaoBanco.Conectar());
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

        public int Alterar(Prontuario prontuario)
        {
            try
            {
                string query = "UPDATE TBPRONTUARIO SET numero = @numero , nome = @nome , sexo = @sexo, nomemae = @nomemae, nomepai = @nomepai, datanascimento = @datanascimento, idade = @idade, classificacao = @classificacao, logradouro = @logradouro, bairro = @bairro, cep = @cep, cidade = @cidade, uf = @uf, dataregistro = @dataregistro, telefoneFixo = @telefoneFixo, telefoneCelular = @telefoneCelular, origemEncaminhamento = @origemEncaminhamento, medicoResponsavel = @medicoResponsavel, observacao = @observacao WHERE codigo = @codigo";

                SqlCommand command = new SqlCommand(query, conexaoBanco.Conectar());
                command.Parameters.AddWithValue("@codigo", prontuario.codigo);
                command.Parameters.AddWithValue("@numero", prontuario.numero);
                command.Parameters.AddWithValue("@nome", prontuario.nome);
                command.Parameters.AddWithValue("@sexo", prontuario.sexo);
                command.Parameters.AddWithValue("@nomemae", prontuario.nomeMae);
                command.Parameters.AddWithValue("@nomepai", prontuario.nomePai);
                command.Parameters.AddWithValue("@datanascimento", prontuario.dataNascimento);
                command.Parameters.AddWithValue("@idade", prontuario.idade);
                command.Parameters.AddWithValue("@classificacao", prontuario.classificacao);
                command.Parameters.AddWithValue("@logradouro", prontuario.logradouro);
                command.Parameters.AddWithValue("@bairro", prontuario.bairro);
                command.Parameters.AddWithValue("@cep", prontuario.cep);
                command.Parameters.AddWithValue("@cidade", prontuario.cidade);
                command.Parameters.AddWithValue("@uf", prontuario.uf);
                command.Parameters.AddWithValue("@dataregistro", DateTime.Today.ToShortDateString());
                command.Parameters.AddWithValue("@telefoneFixo", prontuario.telefoneFixo);
                command.Parameters.AddWithValue("@telefoneCelular", prontuario.telefoneCelular);
                command.Parameters.AddWithValue("@origemEncaminhamento", prontuario.origemEncaminhamento);
                command.Parameters.AddWithValue("@medicoResponsavel", prontuario.medicoResponsavel);
                command.Parameters.AddWithValue("@observacao", prontuario.observacao);

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
