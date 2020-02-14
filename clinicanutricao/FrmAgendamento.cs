using System;
using System.Windows.Forms;
using Model;
using Controller;

namespace View
{
    public partial class FrmAgendamento : Form
    {
        Agendamento agendamentoSelecionado;
        Acao acaoSelecionada;
        AgendamentoController agendamentoController = new AgendamentoController();

        public FrmAgendamento(Agendamento agendamento, Acao acao)
        {
            InitializeComponent();

            this.agendamentoSelecionado = agendamento;
            this.acaoSelecionada = acao;

            switch (acao)
            {
                case Acao.Cadastrar:
                    lblAcao.Text = "AGENDAR";
                    btnSalvar.Text = "Novo";
                    lblDataRegistro.Visible = false;
                    break;
                case Acao.Atualizar:
                    lblAcao.Text = "ATUALIZAR";
                    btnSalvar.Text = "Alterar";
                    Carregar();
                    lblDataRegistro.Visible = true;
                    break;
                case Acao.Visualizar:
                    lblAcao.Text = "VISUALIZAR";
                    Carregar();
                    btnSalvar.Visible = false;
                    lblDataRegistro.Visible = true;
                    break;
            }
        }

        private void rbRetorno_CheckedChanged(object sender, EventArgs e)
        {
            btnSelecionar.Visible = true;
            txtNome.ReadOnly = true;
            txtTelefoneCelular.ReadOnly = true;
            txtTelefoneFixo.ReadOnly = true;
            txtDataNascimento.ReadOnly = true;

            txtTelefoneCelular.Mask = "";
            txtTelefoneFixo.Mask = "";
            txtDataNascimento.Mask = "";
        }

        private void rbCasoNovo_CheckedChanged(object sender, EventArgs e)
        {
            btnSelecionar.Visible = false;
            txtNome.ReadOnly = false;
            txtTelefoneCelular.ReadOnly = false;
            txtTelefoneFixo.ReadOnly = false;
            txtDataNascimento.ReadOnly = false;



            txtTelefoneCelular.Mask = "(##) #####-####";
            txtTelefoneFixo.Mask = "(##) ####-####";
            txtDataNascimento.Mask = "##/##/####";
        }

        void Cadastrar()
        {
            if (btnSalvar.Text == "Novo")
            {
                btnSalvar.Text = "Agendar";
                pnlDados.Enabled = true;
                txtNome.Focus();
            }
            else
            {
                if (Validar())
                {
                    try
                    {
                        Agendamento agendamento = new Agendamento();
                        agendamento.Prontuario = new Prontuario();

                        if (rbRetorno.Checked)
                            agendamento.Prontuario.numero = lblCodigoProntuario.Text;
                        else
                            agendamento.Prontuario.numero = "0";

                        agendamento.Prontuario.nome = txtNome.Text;

                        if (txtTelefoneFixo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 0)
                            agendamento.Prontuario.telefoneFixo = txtTelefoneFixo.Text;
                        else
                            agendamento.Prontuario.telefoneFixo = " ";
                        if (txtTelefoneCelular.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 0)
                            agendamento.Prontuario.telefoneCelular = txtTelefoneCelular.Text;
                        else
                            agendamento.Prontuario.telefoneCelular = " ";


                        if (rbCasoNovo.Checked)
                            agendamento.tipo = "Caso Novo";
                        else
                            agendamento.tipo = "Retorno";

                        agendamento.Prontuario.dataNascimento = Convert.ToDateTime(txtDataNascimento.Text);
                        agendamento.data_agendamento = mtcCalendario.SelectionStart;
                        agendamento.hora = cbxHorario.SelectedItem.ToString();
                        agendamento.consultorio = cbxConsultorio.SelectedItem.ToString();
                        agendamento.observacoes = txtObservacoes.Text;

                        int codigo = agendamentoController.Cadastrar(agendamento);

                        MessageBox.Show("Agendamento salvo com sucesso!", "Agendamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Algo deu errado: " + erro.Message, "Op's", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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

            if (mtcCalendario.SelectionStart < DateTime.Today)
            {
                MessageBox.Show("Você não pode selecionar uma data anterior a data de hoje", "Op'!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                mtcCalendario.SelectionStart = DateTime.Today;
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


            if (cbxHorario.SelectedIndex < 0)
            {
                errorProvider.SetError(cbxHorario, "Selecione um horário");
                errorProvider.SetIconPadding(cbxHorario, -20);
                cbxHorario.Focus();
                return false;
            }

            if (cbxConsultorio.SelectedIndex < 0)
            {
                errorProvider.SetError(cbxConsultorio, "Selecione um consultório");
                errorProvider.SetIconPadding(cbxConsultorio, -20);
                cbxHorario.Focus();
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
                case Acao.Atualizar:
                    Alterar();
                    break;
            }
        }

        private void txtDataNascimento_Leave(object sender, EventArgs e)
        {
            CalculaIdade();
        }

        void CalculaIdade()
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

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            FrmPacientePesquisar frmPacientePesquisar = new FrmPacientePesquisar(Acao.Selecionar);
            if (frmPacientePesquisar.ShowDialog() == DialogResult.OK)
            {
                lblCodigoProntuario.Text = frmPacientePesquisar.prontuario.codigo.ToString();
                txtNome.Text = frmPacientePesquisar.prontuario.nome;

                if (string.IsNullOrEmpty(frmPacientePesquisar.prontuario.telefoneCelular.Replace("-", "").Replace("(", "").Replace(")", "").Trim()))
                    txtTelefoneCelular.Mask = null;
                else
                    txtTelefoneCelular.Text = frmPacientePesquisar.prontuario.telefoneCelular;

                if (string.IsNullOrEmpty(frmPacientePesquisar.prontuario.telefoneFixo.Replace("-", "").Replace("(", "").Replace(")", "").Trim()))
                    txtTelefoneFixo.Mask = null;
                else
                    txtTelefoneFixo.Text = frmPacientePesquisar.prontuario.telefoneFixo;

                txtDataNascimento.Text = frmPacientePesquisar.prontuario.dataNascimento.ToShortDateString();
                CalculaIdade();
            }

        }

        void Carregar()
        {
            txtNome.Text = this.agendamentoSelecionado.Prontuario.nome;
            txtTelefoneFixo.Text = this.agendamentoSelecionado.Prontuario.telefoneFixo;
            txtTelefoneCelular.Text = this.agendamentoSelecionado.Prontuario.telefoneCelular;
            txtDataNascimento.Text = this.agendamentoSelecionado.Prontuario.dataNascimento.ToShortDateString();
            CalculaIdade();
            cbxHorario.SelectedItem = this.agendamentoSelecionado.hora;
            cbxConsultorio.SelectedItem = this.agendamentoSelecionado.consultorio;
            mtcCalendario.SelectionStart = this.agendamentoSelecionado.data_agendamento;
            txtObservacoes.Text = this.agendamentoSelecionado.observacoes;

            lblDataRegistro.Text = "Data do Registro: " + this.agendamentoSelecionado.data_registro.ToShortDateString();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Alterar()
        {
            if (btnSalvar.Text == "Alterar")
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
                        Agendamento agendamento = new Agendamento();
                        agendamento.Prontuario = new Prontuario();
                        agendamento.codigo = this.agendamentoSelecionado.codigo;

                        if (rbRetorno.Checked)
                            agendamento.Prontuario.numero = lblCodigoProntuario.Text;
                        else
                            agendamento.Prontuario.numero = "0";

                        agendamento.Prontuario.nome = txtNome.Text;

                        if (txtTelefoneFixo.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 0)
                            agendamento.Prontuario.telefoneFixo = txtTelefoneFixo.Text;
                        else
                            agendamento.Prontuario.telefoneFixo = " ";
                        if (txtTelefoneCelular.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length > 0)
                            agendamento.Prontuario.telefoneCelular = txtTelefoneCelular.Text;
                        else
                            agendamento.Prontuario.telefoneCelular = " ";


                        if (rbCasoNovo.Checked)
                            agendamento.tipo = "Caso Novo";
                        else
                            agendamento.tipo = "Retorno";

                        agendamento.Prontuario.dataNascimento = Convert.ToDateTime(txtDataNascimento.Text);
                        agendamento.data_agendamento = mtcCalendario.SelectionStart;
                        agendamento.hora = cbxHorario.SelectedItem.ToString();
                        agendamento.consultorio = cbxConsultorio.SelectedItem.ToString();
                        agendamento.observacoes = txtObservacoes.Text;

                        int codigo = agendamentoController.Alterar(agendamento);

                        MessageBox.Show("Agendamento salvo com sucesso!", "Agendamento", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show("Algo deu errado: " + erro.Message, "Op's", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
