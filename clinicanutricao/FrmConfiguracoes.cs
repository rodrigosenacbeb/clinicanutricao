using System;
using System.Windows.Forms;
using Controller;
using Model;

namespace View
{
    public partial class FrmConfiguracoes : Form
    {
        ConexaoBancoDadosController conexaoBancoDadosController = new ConexaoBancoDadosController();

        public FrmConfiguracoes()
        {
            InitializeComponent();
            Configuracoes configuracoes = conexaoBancoDadosController.CarregarConfiguracoes();
            txtServidor.Text = configuracoes.Servidor;
            txtBancoDados.Text = configuracoes.BancoDados;
            txtUsuarioBanco.Text = configuracoes.UsuarioBanco;
            txtSenhaBanco.Text = configuracoes.SenhaBanco;          
            txtNumeroProntuario.Text = configuracoes.numeroProntuario;          
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (btnSalvar.Text == "Alterar")
            {
                pnlDados.Enabled = true;
                btnSalvar.Text = "Salvar";
            }
            else
            {
                if (Validar())
                {
                    Configuracoes configuracoes = new Configuracoes();
                    configuracoes.Servidor = txtServidor.Text.Trim();
                    configuracoes.BancoDados = txtBancoDados.Text.Trim();
                    configuracoes.UsuarioBanco = txtUsuarioBanco.Text.Trim();
                    configuracoes.SenhaBanco = txtSenhaBanco.Text.Trim();                    
                    configuracoes.numeroProntuario = txtNumeroProntuario.Text.Trim();                    

                    try
                    {
                        bool retorno = conexaoBancoDadosController.SalvarConfiguracoes(configuracoes);

                        if (retorno)
                        {
                            MessageBox.Show("Configurações salvas com sucesso!", "Configurações", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Não foi possível salvar as configurações. Detalhes: " + erro.Message, "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        pnlDados.Enabled = false;
                        btnSalvar.Text = "Alterar";
                    }
                }

            }
        }

        bool Validar()
        {
            errorProvider.Clear();

            if (String.IsNullOrWhiteSpace(txtServidor.Text))
            {
                errorProvider.SetError(txtServidor, "Informe o servidor");
                errorProvider.SetIconPadding(txtServidor, -20);
                txtServidor.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtBancoDados.Text))
            {
                errorProvider.SetError(txtBancoDados, "Informe o nome do banco de dados");
                errorProvider.SetIconPadding(txtBancoDados, -20);
                txtBancoDados.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtUsuarioBanco.Text))
            {
                errorProvider.SetError(txtUsuarioBanco, "Informe o usuário do banco de dados");
                errorProvider.SetIconPadding(txtUsuarioBanco, -20);
                txtUsuarioBanco.Focus();
                return false;
            }

            if (txtUsuarioBanco.Text.IndexOf(" ") > 0)
            {
                errorProvider.SetError(txtUsuarioBanco, "Não é permitido criar um usuário com espaços");
                errorProvider.SetIconPadding(txtUsuarioBanco, -20);
                txtUsuarioBanco.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtSenhaBanco.Text))
            {
                errorProvider.SetError(txtSenhaBanco, "Informe a senha do usuário do banco de dados");
                errorProvider.SetIconPadding(txtSenhaBanco, -20);
                txtSenhaBanco.Focus();
                return false;
            }


            if (txtSenhaBanco.Text.IndexOf(" ") > 0)
            {
                errorProvider.SetError(txtSenhaBanco, "Não é permitido criar uma senha com espaços");
                errorProvider.SetIconPadding(txtSenhaBanco, -20);
                txtSenhaBanco.Focus();
                return false;
            }

            long numero;

            if (!long.TryParse(txtNumeroProntuario.Text, out numero))
            {
                errorProvider.SetError(txtNumeroProntuario, "Informe apenas números");
                errorProvider.SetIconPadding(txtNumeroProntuario, -20);
                txtNumeroProntuario.Focus();
                return false;
            }
            else
            {
                if(numero <= 0)
                {
                    errorProvider.SetError(txtNumeroProntuario, "Não é permitido números iguais ou menores que zero");
                    errorProvider.SetIconPadding(txtNumeroProntuario, -20);
                    txtNumeroProntuario.Focus();
                    return false;
                }
            }


            return true;
        }

    }
}
