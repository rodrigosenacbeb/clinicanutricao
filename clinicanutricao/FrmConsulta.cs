using System;
using System.Windows.Forms;
using Model;

namespace View
{
    public partial class FrmConsulta : Form
    {
        public FrmConsulta(Agendamento agendamento)
        {
            InitializeComponent();

            txtNome.Text = agendamento.nome_tmp;                       
            txtNumeroProntuario.Text = agendamento.Prontuario.numero;
            txtData.Text = agendamento.data_agendamento.ToShortDateString();
            cbxHorario.SelectedItem = agendamento.hora;
            cbxConsultorio.SelectedItem = agendamento.consultorio;
        }

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            FrmPacientePesquisar frmPacientePesquisar = new FrmPacientePesquisar(Acao.Selecionar);
            if (frmPacientePesquisar.ShowDialog() == DialogResult.OK)
            {
                lblcodigo.Text = frmPacientePesquisar.prontuario.codigo.ToString();               
            }
        }
    }
}
