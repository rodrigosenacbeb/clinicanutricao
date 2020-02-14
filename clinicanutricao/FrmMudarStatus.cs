using System;
using System.Windows.Forms;

namespace View
{
    public partial class FrmMudarStatus : Form
    {
        public string status;

        public FrmMudarStatus()
        {
            InitializeComponent();
        }

        private void cbxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.status = cbxStatus.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
