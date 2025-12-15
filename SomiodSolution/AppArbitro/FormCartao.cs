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
    public partial class FormCartao : Form
    {
        public CartaoData Result { get; private set; }

        public FormCartao(string equipaA, string equipaB)
        {
            InitializeComponent();

            cbEquipa.DropDownStyle = ComboBoxStyle.DropDownList;
            cbTipoCartao.DropDownStyle = ComboBoxStyle.DropDownList;

            cbEquipa.Items.Add(equipaA);
            cbEquipa.Items.Add(equipaB);
            cbEquipa.SelectedIndex = 0;

            cbTipoCartao.Items.Add("amarelo");
            cbTipoCartao.Items.Add("vermelho");
            cbTipoCartao.SelectedIndex = 0;

            numMinuto.Minimum = 0;
            numMinuto.Maximum = 120;
            numMinuto.Value = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string jogador = txtJogador.Text.Trim();
            if (string.IsNullOrWhiteSpace(jogador))
            {
                MessageBox.Show("O jogador é obrigatório.");
                return;
            }

            string equipa = cbEquipa.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(equipa))
            {
                MessageBox.Show("Escolhe a equipa.");
                return;
            }

            string tipo = cbTipoCartao.SelectedItem?.ToString();
            if (tipo != "amarelo" && tipo != "vermelho")
            {
                MessageBox.Show("Escolhe 'amarelo' ou 'vermelho'.");
                return;
            }

            Result = new CartaoData
            {
                Minuto = (int)numMinuto.Value,
                Jogador = jogador,
                Equipa = equipa,
                TipoCartao = tipo
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
