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
        ProntuarioController prontuarioController = new ProntuarioController();

        public FrmPacienteCadastro(Prontuario prontuario, Acao acao)
        {
            InitializeComponent();

            this.prontuarioSelecionado = prontuario;
            this.acaoSelecionada = acao;

            txtNumeroProntuario.Text = prontuarioController.UltimoNumeroProntuario();
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
                txtNome.Focus();
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
                        prontuario.idade = Convert.ToInt32(txtIdade.Text);
                        prontuario.classificacao = txtClassificacao.Text;

                        if (cbxSexo.SelectedItem.ToString() == "Outros")
                            prontuario.sexo = txtOutroSexo.Text.Trim();
                        else
                            prontuario.sexo = cbxSexo.SelectedItem.ToString();

                        if (txtTelefoneFixo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 0)
                            prontuario.telefoneFixo = txtTelefoneFixo.Text;
                        else
                            prontuario.telefoneFixo = "";
                        if (txtTelefoneCelular.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 0)
                            prontuario.telefoneCelular = txtTelefoneCelular.Text;
                        else
                            prontuario.telefoneCelular = "";                            

                        prontuario.nomeMae = txtNomeMae.Text.Trim();
                        prontuario.nomePai = txtNomePai.Text.Trim();
                        prontuario.cep = txtCep.Text.Replace("-", "");
                        prontuario.logradouro = txtLogradouro.Text.Trim();
                        prontuario.bairro = txtBairro.Text.Trim();
                        prontuario.cidade = txtCidade.Text.Trim();
                        prontuario.uf = cbxUF.SelectedItem.ToString();
                        prontuario.origemEncaminhamento = txtOrigemEncaminhamento.Text.Trim();
                        prontuario.medicoResponsavel = txtMedicoResponsavel.Text.Trim();
                        prontuario.observacao = txtObservacoes.Text.Trim();

                        int codigo = prontuarioController.Cadastrar(prontuario);

                        MessageBox.Show("Prontuário salvo com sucesso!", "Prontuário", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Algo deu errado: " + erro.Message, "Op's", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtDataNascimento_Leave(object sender, EventArgs e)
        {
            DateTime data;

            if (DateTime.TryParse(txtDataNascimento.Text, out data))
            {
                int idade = DateTime.Today.Year - data.Year;

                if (DateTime.Today.Month <= Convert.ToDateTime(txtDataNascimento.Text).Month)
                {
                    idade -= 1;
                }

                if (idade <= 12)
                {
                    txtClassificacao.Text = "Criança";
                }
                else if (idade > 12 && idade <= 19)
                {
                    txtClassificacao.Text = "Adolescente";
                }
                else if (idade > 19 && idade <= 65)
                {
                    txtClassificacao.Text = "Adulto";
                }
                else
                {
                    txtClassificacao.Text = "Idoso";
                }

                txtIdade.Text = idade.ToString();
            }
        }
    }
}