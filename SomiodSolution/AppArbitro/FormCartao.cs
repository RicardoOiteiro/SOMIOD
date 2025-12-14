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
        private readonly SomiodApiClient _api;
        private readonly string _appName;
        private readonly Action<string> _log;

        public FormCartao(string appName, SomiodApiClient api, Action<string> log)
        {
            InitializeComponent();
            _appName = appName;
            _api = api;
            _log = log;

            comboBox1.Items.AddRange(new[] { "amarelo", "vermelho" });
            comboBox1.SelectedIndex = 0;
        }

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            var jogador = txtJogador.Text.Trim();
            var tipo = comboBox1.SelectedItem?.ToString();
            var minuto = (int)numericUpDown1.Value;

            if (string.IsNullOrWhiteSpace(jogador) || string.IsNullOrWhiteSpace(tipo))
            {
                MessageBox.Show("Preenche todos os campos.");
                return;
            }

            var contentJson =
                $@"{{""player"":""{jogador}"",""type"":""{tipo}"",""minute"":{minuto}}}";

            var ciName = $"card-{DateTime.Now.Ticks}";

            var (ok, code, body) = await _api.CreateContentInstanceAsync(_appName, "cards", ciName, "application/json", contentJson);

            _log($"CARTÃO {tipo.ToUpper()}: {jogador} ({minuto}') -> {(int)code} {code}");

            if (!ok)
            {
                MessageBox.Show("Erro ao registar cartão.");
                return;
            }

            Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
