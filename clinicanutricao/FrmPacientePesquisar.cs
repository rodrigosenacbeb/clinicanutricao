using System;
using System.Windows.Forms;
using Controller;
using Model;

namespace View
{
    public partial class FrmPacientePesquisar : Form
    {
        Acao acaoSelecionada;
        public Prontuario prontuario = new Prontuario();

        public FrmPacientePesquisar(Acao acao)
        {
            InitializeComponent();
            this.acaoSelecionada = acao;
            Pesquisar();

            switch (acao)
            {
                case Acao.Selecionar:
                    btnNovo.Visible = false;
                    btnAtualizar.Visible = false;
                    btnVisualizar.Text = "Selecionar";
                    lblAcao.Text = "SELECIONAR";
                    break;
            }
        }

        void Pesquisar()
        {
            ProntuarioController prontuarioController = new ProntuarioController();
            dgvDados.DataSource = prontuarioController.Pesquisar(txtNome.Text);

        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            Pesquisar();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            FrmPacienteCadastro frmPacienteCadastro = new FrmPacienteCadastro(null, Model.Acao.Cadastrar);
            frmPacienteCadastro.ShowDialog();
        }

        void Carregar()
        {
            if (dgvDados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Você precisa selecionar um paciente.", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                prontuario.codigo = Convert.ToInt32(dgvDados.CurrentRow.Cells["codigo"].Value);
                prontuario.numero = dgvDados.CurrentRow.Cells["numero"].Value.ToString();
                prontuario.nome = dgvDados.CurrentRow.Cells["nome"].Value.ToString();
                prontuario.dataNascimento = Convert.ToDateTime(dgvDados.CurrentRow.Cells["datanascimento"].Value);
                prontuario.dataRegistro = Convert.ToDateTime(dgvDados.CurrentRow.Cells["dataregistro"].Value);
                prontuario.idade = Convert.ToInt32(dgvDados.CurrentRow.Cells["idade"].Value);
                prontuario.classificacao = dgvDados.CurrentRow.Cells["classificacao"].Value.ToString();
                prontuario.sexo = dgvDados.CurrentRow.Cells["sexo"].Value.ToString();
                prontuario.telefoneFixo = dgvDados.CurrentRow.Cells["telefoneFixo"].Value.ToString();
                prontuario.telefoneCelular = dgvDados.CurrentRow.Cells["telefoneCelular"].Value.ToString();
                prontuario.nomeMae = dgvDados.CurrentRow.Cells["nomeMae"].Value.ToString();
                prontuario.nomePai = dgvDados.CurrentRow.Cells["nomePai"].Value.ToString();
                prontuario.cep = dgvDados.CurrentRow.Cells["cep"].Value.ToString();
                prontuario.logradouro = dgvDados.CurrentRow.Cells["logradouro"].Value.ToString();
                prontuario.bairro = dgvDados.CurrentRow.Cells["bairro"].Value.ToString();
                prontuario.cidade = dgvDados.CurrentRow.Cells["cidade"].Value.ToString();
                prontuario.uf = dgvDados.CurrentRow.Cells["uf"].Value.ToString();
                prontuario.origemEncaminhamento = dgvDados.CurrentRow.Cells["origemEncaminhamento"].Value.ToString();
                prontuario.medicoResponsavel = dgvDados.CurrentRow.Cells["medicoResponsavel"].Value.ToString();
                prontuario.observacao = dgvDados.CurrentRow.Cells["observacao"].Value.ToString();

                FrmPacienteCadastro frmPacienteCadastro;

                switch (this.acaoSelecionada)
                {
                    case Acao.Atualizar:
                        frmPacienteCadastro = new FrmPacienteCadastro(prontuario, Acao.Atualizar);
                        frmPacienteCadastro.ShowDialog();
                        Pesquisar();
                        break;
                    case Acao.Visualizar:
                        frmPacienteCadastro = new FrmPacienteCadastro(prontuario, Acao.Visualizar);
                        frmPacienteCadastro.ShowDialog();
                        Pesquisar();
                        break;
                    case Acao.Selecionar:
                        this.DialogResult = DialogResult.OK;
                        return;

                }
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            Carregar();
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            Carregar();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
