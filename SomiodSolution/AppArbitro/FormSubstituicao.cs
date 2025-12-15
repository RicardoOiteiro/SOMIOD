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
    public partial class FormSubstituicao : Form
    {
        public SubData Result { get; private set; }
        public FormSubstituicao(string equipaA, string equipaB)
        {
            InitializeComponent();

            cbEquipa.DropDownStyle = ComboBoxStyle.DropDownList;

            cbEquipa.Items.Add(equipaA);
            cbEquipa.Items.Add(equipaB);
            cbEquipa.SelectedIndex = 0;

            numMinuto.Minimum = 0;
            numMinuto.Maximum = 120;
            numMinuto.Value = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string sai = txtSai.Text.Trim();
            string entra = txtEntra.Text.Trim();

            if (string.IsNullOrWhiteSpace(sai) || string.IsNullOrWhiteSpace(entra))
            {
                MessageBox.Show("Tens de indicar quem sai e quem entra.");
                return;
            }

            Result = new SubData
            {
                Minuto = (int)numMinuto.Value,
                Sai = sai,
                Entra = entra,
                Equipa = cbEquipa.SelectedItem?.ToString()
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
