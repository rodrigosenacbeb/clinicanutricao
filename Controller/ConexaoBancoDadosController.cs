using System;
using System.Data.SqlClient;
using System.Data;
using Model;

namespace Controller
{
    public class ConexaoBancoDadosController
    {
        string parametros = string.Format(@"Data Source={0};Initial Catalog={1};User ID={2};Password={3}", 
            Properties.Settings.Default.servidor,
            Properties.Settings.Default.bancoDados,
            Properties.Settings.Default.usuarioBanco,
            Properties.Settings.Default.senhaBanco);

        SqlConnection conexao = null;

        public bool SalvarConfiguracoes(Configuracoes configuracoes)
        {
            try
            {
                Properties.Settings.Default.servidor = configuracoes.Servidor;
                Properties.Settings.Default.bancoDados = configuracoes.BancoDados;
                Properties.Settings.Default.usuarioBanco = configuracoes.UsuarioBanco;
                Properties.Settings.Default.senhaBanco = configuracoes.SenhaBanco;
                Properties.Settings.Default.numeroProntuario  = configuracoes.numeroProntuario;


                Properties.Settings.Default.Save();


                return true;
            }
            catch (Exception erro)
            {
                throw erro;
            }
        }

        public SqlConnection Conectar()
        {
            try
            {
                conexao = new SqlConnection(parametros);
                if(conexao.State == ConnectionState.Closed)
                {
                    conexao.Open();
                }

                return conexao;
                
            }
            catch (Exception erro)
            {
                conexao.Close();
                throw erro;
            }
        }

        public void Fechar()
        {
            conexao.Close();
        }

        public Configuracoes CarregarConfiguracoes()
        {
            Configuracoes configuracoes = new Configuracoes();
            configuracoes.Servidor = Properties.Settings.Default.servidor;
            configuracoes.BancoDados = Properties.Settings.Default.bancoDados;
            configuracoes.UsuarioBanco = Properties.Settings.Default.usuarioBanco;
            configuracoes.SenhaBanco = Properties.Settings.Default.senhaBanco;            
            configuracoes.numeroProntuario = Properties.Settings.Default.numeroProntuario;            

            return configuracoes;
        }
    }
}
