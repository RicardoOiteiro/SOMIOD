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
        private readonly SomiodApiClient _api;
        private readonly string _appName;
        private readonly Action<string> _log;

        public FormSubstituicao(string appName, SomiodApiClient api, Action<string> log)
        {
            InitializeComponent();
            _appName = appName;
            _api = api;
            _log = log;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            var entra = txtEntra.Text.Trim();
            var sai = txtSai.Text.Trim();
            var minuto = (int)numericUpDown1.Value;

            if (string.IsNullOrWhiteSpace(entra) || string.IsNullOrWhiteSpace(sai))
            {
                MessageBox.Show("Preenche todos os campos.");
                return;
            }

            var contentJson =
                $@"{{""in"":""{entra}"",""out"":""{sai}"",""minute"":{minuto}}}";

            var ciName = $"sub-{DateTime.Now.Ticks}";

            var (ok, code, body) = await _api.CreateContentInstanceAsync(_appName, "substitutions", ciName, "application/json", contentJson);

            _log($"SUBSTITUIÇÃO: entra {entra}, sai {sai} ({minuto}') -> {(int)code} {code}");

            if (!ok)
            {
                MessageBox.Show("Erro ao registar substituição.");
                return;
            }

            Close();
        }
    }
}
