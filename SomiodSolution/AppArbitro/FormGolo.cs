using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppArbitro
{
    public partial class FormGolo : Form
    {
        public GoloData Data { get; private set; }

        public FormGolo(string equipaA, string equipaB)
        {
            InitializeComponent();

            cmbEquipa.Items.Add(equipaA);
            cmbEquipa.Items.Add(equipaB);
            cmbEquipa.SelectedIndex = 0;

            numMinuto.Minimum = 0;
            numMinuto.Maximum = 120;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var jogador = txtJogador.Text.Trim();
            if (string.IsNullOrWhiteSpace(jogador))
            {
                MessageBox.Show("Jogador é obrigatório.");
                return;
            }

            Data = new GoloData
            {
                Minuto = (int)numMinuto.Value,
                Jogador = jogador,
                Equipa = cmbEquipa.SelectedItem.ToString()
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
