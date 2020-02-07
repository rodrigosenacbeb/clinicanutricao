using System;
using System.IO;
using System.Windows.Forms;
using Model;
using Controller;

namespace View
{
    public partial class FrmPacienteCadastro : Form
    {
        Acao acaoSelecionada;
        Prontuario prontuarioSelecionado;

        public FrmPacienteCadastro(Prontuario prontuario, Acao acao)
        {
            InitializeComponent();

            this.prontuarioSelecionado = prontuario;
            this.acaoSelecionada = acao;
        }

        private void btnBuscarCep_Click(object sender, EventArgs e)
        {

            try
            {

                if (String.IsNullOrWhiteSpace(txtCep.Text.Replace("-", "")) || txtCep.Text.Replace("-", "").Length < 8)
                {
                    MessageBox.Show("Informe um CEP válido!");
                    return;
                }


                System.Net.HttpWebRequest requisicao = (System.Net.HttpWebRequest)System.Net.WebRequest.Create("https://viacep.com.br/ws/" + txtCep.Text.Replace("-", "") + "/json/");

                requisicao.AllowAutoRedirect = false;
                System.Net.HttpWebResponse ChecaServidor = (System.Net.HttpWebResponse)requisicao.GetResponse();

                if (ChecaServidor.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Servidor indisponível");
                    return;
                }


                using (Stream webStream = ChecaServidor.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            string response = responseReader.ReadToEnd();
                            response = System.Text.RegularExpressions.Regex.Replace(response, "[{},]", string.Empty);
                            response = response.Replace("\"", "");

                            String[] substrings = response.Split('\n');

                            int cont = 0;
                            foreach (var substring in substrings)
                            {
                                if (cont == 1)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    if (valor[0] == "  erro")
                                    {
                                        MessageBox.Show("CEP não encontrado");
                                        txtCep.Focus();
                                        return;
                                    }
                                }

                                //Logradouro
                                if (cont == 2)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txtLogradouro.Text = valor[1];
                                }

                                //Bairro
                                if (cont == 4)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txtBairro.Text = valor[1];
                                }

                                //Localidade (Cidade)
                                if (cont == 5)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    txtCidade.Text = valor[1];
                                }

                                //Estado (UF)
                                if (cont == 6)
                                {
                                    string[] valor = substring.Split(":".ToCharArray());
                                    cbxUF.SelectedItem = valor[1].Trim();
                                }

                                cont++;
                            }
                        }
                    }
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show("Algo deu errado. Detalhes: " + erro.Message);
            }

        }

        bool Validar()
        {
            errorProvider.Clear();

            // Se todos os campos obrigatórios estiverem preenchidos vai retornar TRUE.
            if (String.IsNullOrWhiteSpace(txtNome.Text))
            {
                errorProvider.SetError(txtNome, "Informe o nome");
                errorProvider.SetIconPadding(txtNome, -20);
                txtNome.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtDataNascimento.Text.Replace("/", "")) || txtDataNascimento.Text.Replace("/", "").Length < 8)
            {
                errorProvider.SetError(txtDataNascimento, "Informe a data de nascimento");
                errorProvider.SetIconPadding(txtDataNascimento, -20);
                txtDataNascimento.Focus();
                return false;
            }
            else
            {
                DateTime result;
                if (!DateTime.TryParse(txtDataNascimento.Text, out result))
                {
                    errorProvider.SetError(txtDataNascimento, "Informe uma data de nascimento válida");
                    errorProvider.SetIconPadding(txtDataNascimento, -20);
                    txtDataNascimento.Focus();
                    return false;
                }
            }

            if (cbxSexo.SelectedIndex < 0)
            {
                errorProvider.SetError(cbxSexo, "Informe o sexo");
                errorProvider.SetIconPadding(cbxSexo, -20);
                cbxSexo.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtLogradouro.Text))
            {
                errorProvider.SetError(txtLogradouro, "Informe o endereço");
                errorProvider.SetIconPadding(txtLogradouro, -20);
                txtLogradouro.Focus();
                return false;
            }


            if (String.IsNullOrWhiteSpace(txtTelefoneCelular.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim()) && String.IsNullOrWhiteSpace(txtTelefoneFixo.Text.Replace("-", "").Replace("(", "").Replace(")", "").Trim()))
            {
                errorProvider.SetError(txtTelefoneCelular, "Informe um número de telefone fixo ou celular");
                errorProvider.SetError(txtTelefoneFixo, "Informe um número de telefone fixo ou celular");

                errorProvider.SetIconPadding(txtTelefoneCelular, -20);
                errorProvider.SetIconPadding(txtTelefoneFixo, -20);

                txtTelefoneFixo.Focus();
                return false;
            }

            return true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            switch (this.acaoSelecionada)
            {
                case Acao.Cadastrar:
                    Cadastrar();
                    break;
            }

        }

        void Cadastrar()
        {
            if (btnSalvar.Text == "Novo")
            {
                btnSalvar.Text = "Salvar";
                pnlDados.Enabled = true;
            }
            else
            {
                if (Validar())
                {
                    try
                    {
                        Prontuario prontuario = new Prontuario();
                        prontuario.numero = txtNumeroProntuario.Text.Trim();
                        prontuario.nome = txtNome.Text.Trim();
                        prontuario.dataNascimento = Convert.ToDateTime(txtDataNascimento.Text);

                        if (cbxSexo.SelectedItem.ToString() == "Outros")
                            prontuario.sexo = txtOutroSexo.Text.Trim();
                        else
                            prontuario.sexo = cbxSexo.SelectedItem.ToString();

                        prontuario.telefoneFixo = txtTelefoneFixo.Text;
                        prontuario.telefoneCelular = txtTelefoneCelular.Text;
                        prontuario.nomeMae = txtNomeMae.Text.Trim();
                        prontuario.nomePai = txtNomePai.Text.Trim();
                        prontuario.CEP = txtCep.Text;
                        prontuario.Logradouro = txtLogradouro.Text.Trim();
                        prontuario.Bairro = txtBairro.Text.Trim();
                        prontuario.Cidade = txtCidade.Text.Trim();
                        prontuario.UF = cbxUF.SelectedItem.ToString();
                        prontuario.OrigemEncaminhamento = txtOrigemEncaminhamento.Text.Trim();
                        prontuario.MedicoResponsavel = txtMedicoResponsavel.Text.Trim();
                        prontuario.Observacao = txtObservacoes.Text.Trim();

                        ProntuarioController prontuarioController = new ProntuarioController();
                        string codigo = prontuarioController.Cadastrar(prontuario);

                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Algo deu errado: " + erro.Message);
                    }
                }
            }
        }

        private void cbxSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSexo.SelectedItem.ToString() == "Outros")
            {
                txtOutroSexo.Enabled = true;
            }
            else
            {
                txtOutroSexo.Enabled = false;
                txtOutroSexo.Clear();
            }
        }
    }
}