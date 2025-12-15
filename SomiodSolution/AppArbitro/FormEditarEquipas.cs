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
    public partial class FormEditarEquipas : Form
    {
        public string EquipaA { get; private set; }
        public string EquipaB { get; private set; }

        public FormEditarEquipas(string equipaAOld, string equipaBOld)
        {
            InitializeComponent();

            txtEquipaA.Text = equipaAOld;
            txtEquipaB.Text = equipaBOld;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string a = txtEquipaA.Text.Trim();
            string b = txtEquipaB.Text.Trim();

            if (string.IsNullOrWhiteSpace(a) || string.IsNullOrWhiteSpace(b))
            {
                MessageBox.Show("As duas equipas são obrigatórias.");
                return;
            }

            EquipaA = a;
            EquipaB = b;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
