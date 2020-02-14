using System;
using System.Windows.Forms;
using Model;
using Controller;

namespace View
{
    public partial class FrmAgendamentoPesquisar : Form
    {
        Acao acaoSelecionada;
        Agendamento agendamento = new Agendamento();

        public FrmAgendamentoPesquisar()
        {
            InitializeComponent();
            dtpAte.Value = dtpAte.Value.AddDays(7);

            Pesquisar();
        }

        void Pesquisar()
        {

            AgendamentoController agendamentoController = new AgendamentoController();
            dgvDados.DataSource = agendamentoController.Pesquisar(txtNome.Text, dtpDe.Value, dtpAte.Value);

        }

        private void dtpAte_ValueChanged(object sender, EventArgs e)
        {
            Pesquisar();
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            Pesquisar();
        }

        void Carregar()
        {
            if (dgvDados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Você precisa selecionar um agendamento.", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                agendamento.codigo = Convert.ToInt32(dgvDados.CurrentRow.Cells["codigo"].Value);
                agendamento.Prontuario = new Prontuario();
                agendamento.Prontuario.numero = dgvDados.CurrentRow.Cells["codigo_prontuario"].Value.ToString();
                agendamento.Prontuario.nome = dgvDados.CurrentRow.Cells["nome_tmp"].Value.ToString();
                agendamento.Prontuario.telefoneCelular = dgvDados.CurrentRow.Cells["telcelular_tmp"].Value.ToString();
                agendamento.Prontuario.telefoneFixo = dgvDados.CurrentRow.Cells["telfixo_tmp"].Value.ToString();
                agendamento.Prontuario.dataNascimento = Convert.ToDateTime(dgvDados.CurrentRow.Cells["data_nascimento"].Value);
                agendamento.data_agendamento = Convert.ToDateTime(dgvDados.CurrentRow.Cells["data_agendamento"].Value);
                agendamento.hora = dgvDados.CurrentRow.Cells["hora"].Value.ToString();
                agendamento.consultorio = dgvDados.CurrentRow.Cells["consultorio"].Value.ToString();
                agendamento.observacoes = dgvDados.CurrentRow.Cells["observacoes"].Value.ToString();
                agendamento.data_registro = Convert.ToDateTime(dgvDados.CurrentRow.Cells["data_registro"].Value);
                agendamento.tipo = dgvDados.CurrentRow.Cells["tipo"].Value.ToString();
                agendamento.status = dgvDados.CurrentRow.Cells["status"].Value.ToString();

                FrmAgendamento frmAgendamento;

                switch (this.acaoSelecionada)
                {
                    case Acao.Atualizar:
                        frmAgendamento = new FrmAgendamento(agendamento, Acao.Atualizar);
                        frmAgendamento.ShowDialog();
                        Pesquisar();
                        break;
                    case Acao.Visualizar:
                        frmAgendamento = new FrmAgendamento(agendamento, Acao.Visualizar);
                        frmAgendamento.ShowDialog();
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
            this.acaoSelecionada = Acao.Atualizar;
            Carregar();
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            this.acaoSelecionada = Acao.Visualizar;
            Carregar();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            FrmAgendamento frmAgendamento = new FrmAgendamento(null, Acao.Cadastrar);
            frmAgendamento.ShowDialog();
            Pesquisar();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Você precisa selecionar um agendamento para excluír.", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (MessageBox.Show("Deseja realmente excluír esse agendamento?", "Agendamento", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AgendamentoController agendamentoController = new AgendamentoController();
                dgvDados.DataSource = agendamentoController.Excluir(Convert.ToInt32(Convert.ToInt32(dgvDados.CurrentRow.Cells["codigo"].Value)));
                Pesquisar();
            }
        }

        private void dgvDados_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0)
            {
                MessageBox.Show("Você precisa selecionar um agendamento para mudar o status.", "Op's!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                FrmMudarStatus frmMudarStatus = new FrmMudarStatus();
                if (frmMudarStatus.ShowDialog() == DialogResult.OK)
                {
                    AgendamentoController agendamentoController = new AgendamentoController();
                    agendamentoController.MudarStatus(Convert.ToInt32(Convert.ToInt32(dgvDados.CurrentRow.Cells["codigo"].Value)), frmMudarStatus.status);
                    Pesquisar();
                }
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
