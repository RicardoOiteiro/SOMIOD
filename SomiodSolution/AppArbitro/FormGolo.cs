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
        private readonly SomiodApiClient _api;
        private readonly string _appName;
        private readonly Action<string> _log;

        public FormGolo(string appName, SomiodApiClient api, Action<string> log)
        {
            InitializeComponent();
            _appName = appName;
            _api = api;
            _log = log;
        }

        private async void btnConfirmar_Click(object sender, EventArgs e)
        {
            var equipa = txtEquipa.Text.Trim();
            var jogador = txtJogador.Text.Trim();
            var minuto = (int)numericUpDown1.Value;

            if (equipa == "" || jogador == "")
            {
                MessageBox.Show("Preenche todos os campos.");
                return;
            }

            var contentJson = $@"{{""team"":""{equipa}"",""player"":""{jogador}"",""minute"":{minuto}}}";
            var ciName = $"goal-{DateTime.Now.Ticks}";

            var (ok, code, body) = await _api.CreateContentInstanceAsync(
                _appName, "goals", ciName, "application/json", contentJson);

            _log($"GOLO: {equipa} - {jogador} ({minuto}') -> {(int)code} {code} | {body}");

            if (!ok)
            {
                MessageBox.Show("Falhou criar content-instance. Vê o log.");
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
