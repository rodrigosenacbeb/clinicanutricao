using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public partial class FrmPacienteCadastro : Form
    {
        public FrmPacienteCadastro()
        {
            InitializeComponent();
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
                errorProvider.SetError(txtNome, "Informe o nome do paciente");
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

                }
            }





            return true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Validar();
        }
    }
}