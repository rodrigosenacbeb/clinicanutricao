using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FrmPacienteCadastro(null, Model.Acao.Cadastrar));
            //Application.Run(new FrmConfiguracoes());
            //Application.Run(new FrmAgendamento(null, Model.Acao.Cadastrar));
           // Application.Run(new FrmPacientePesquisar());
           Application.Run(new FrmAgendamentoPesquisar());
        }
    }
}
