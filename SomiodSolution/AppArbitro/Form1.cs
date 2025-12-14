using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace AppArbitro
{
    public partial class Form1 : Form
    {
        private const string BaseUrl = "http://localhost:52047/";

        private string _appName = null;
        private const string ContainerName = "eventos";

        private int _golos = 0;
        private int _cartoes = 0;
        private int _subs = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnIniciarJogo_Click(object sender, EventArgs e)
        {
            string equipaA = txtEquipaA.Text.Trim();
            string equipaB = txtEquipaB.Text.Trim();

            if (string.IsNullOrWhiteSpace(equipaA) || string.IsNullOrWhiteSpace(equipaB))
            {
                MessageBox.Show("Preenche EQUIPA A e EQUIPA B.");
                return;
            }

            _appName = BuildAppName(equipaA, equipaB);

            btnIniciarJogo.Enabled = false;

            // 1) criar application
            bool okApp = await CreateApplicationAsync(_appName);
            if (!okApp)
            {
                _appName = null;
                btnIniciarJogo.Enabled = true;
                return;
            }

            // 2) criar container "eventos"
            bool okCont = await CreateContainerAsync(_appName, ContainerName);
            if (!okCont)
            {
                MessageBox.Show("Application criada, mas falhou a criação do container 'eventos'.");
                _appName = null;
                btnIniciarJogo.Enabled = true;
                return;
            }

            // reset contadores
            _golos = 0;
            _cartoes = 0;
            _subs = 0;

            // UI
            lblJogoAtual.Text = $"Jogo atual: {_appName}";
            SetJogoUIVisible(true);

            txtEquipaA.Enabled = false;
            txtEquipaB.Enabled = false;

            MessageBox.Show("Jogo iniciado com sucesso!");
        }

        private async void btnTerminar_Click(object sender, EventArgs e)
        {
            if (_appName == null) return;

            btnTerminar.Enabled = false;

            bool ok = await TerminateGameCascadeAsync(_appName);

            if (ok)
            {
                _appName = null;
                lblJogoAtual.Text = "";
                SetJogoUIVisible(false);

                txtEquipaA.Enabled = true;
                txtEquipaB.Enabled = true;
                btnIniciarJogo.Enabled = true;
                btnTerminar.Enabled = true;

                txtEquipaA.Clear();
                txtEquipaB.Clear();

                MessageBox.Show("Jogo terminado.");
                return;
            }

            btnTerminar.Enabled = true;

            Console.WriteLine("Jogo terminado.");
        }

        private async void btnGolo_Click(object sender, EventArgs e)
        {
            if (_appName == null) return;

            string minutoStr = Interaction.InputBox("Minuto do golo?", "GOLO", "0");
            if (!int.TryParse(minutoStr, out int minuto) || minuto < 0 || minuto > 130)
            {
                MessageBox.Show("Minuto inválido.");
                return;
            }

            string jogador = Interaction.InputBox("Nome do jogador?", "GOLO", "");
            if (string.IsNullOrWhiteSpace(jogador))
            {
                MessageBox.Show("Jogador é obrigatório.");
                return;
            }

            _golos++;
            string ciName = $"golo{_golos}";

            var payload = new
            {
                tipo = "golo",
                minuto = minuto,
                jogador = jogador.Trim(),
                equipaA = txtEquipaA.Text.Trim(),
                equipaB = txtEquipaB.Text.Trim(),
                timestamp = DateTime.UtcNow.ToString("o")
            };

            bool ok = await PublishEventAsync(ciName, payload);
            if (ok) MessageBox.Show($"Evento publicado: {ciName}");
        }

        private async void btnCartao_Click(object sender, EventArgs e)
        {
            if (_appName == null) return;

            string minutoStr = Interaction.InputBox("Minuto do cartão?", "CARTÃO", "0");
            if (!int.TryParse(minutoStr, out int minuto) || minuto < 0 || minuto > 130)
            {
                MessageBox.Show("Minuto inválido.");
                return;
            }

            string jogador = Interaction.InputBox("Nome do jogador?", "CARTÃO", "");
            if (string.IsNullOrWhiteSpace(jogador))
            {
                MessageBox.Show("Jogador é obrigatório.");
                return;
            }

            string tipoCartao = Interaction.InputBox("Tipo de cartão? (amarelo/vermelho)", "CARTÃO", "amarelo").ToLowerInvariant();
            if (tipoCartao != "amarelo" && tipoCartao != "vermelho")
            {
                MessageBox.Show("Tipo de cartão inválido (usa amarelo ou vermelho).");
                return;
            }

            _cartoes++;
            string ciName = $"cartao{_cartoes}";

            var payload = new
            {
                tipo = "cartao",
                minuto = minuto,
                jogador = jogador.Trim(),
                cartao = tipoCartao,
                equipaA = txtEquipaA.Text.Trim(),
                equipaB = txtEquipaB.Text.Trim(),
                timestamp = DateTime.UtcNow.ToString("o")
            };

            bool ok = await PublishEventAsync(ciName, payload);
            if (ok) MessageBox.Show($"Evento publicado: {ciName}");
        }

        private async void btnSub_Click(object sender, EventArgs e)
        {
            if (_appName == null) return;

            string minutoStr = Interaction.InputBox("Minuto da substituição?", "SUBSTITUIÇÃO", "0");
            if (!int.TryParse(minutoStr, out int minuto) || minuto < 0 || minuto > 130)
            {
                MessageBox.Show("Minuto inválido.");
                return;
            }

            string sai = Interaction.InputBox("Jogador que sai?", "SUBSTITUIÇÃO", "");
            if (string.IsNullOrWhiteSpace(sai))
            {
                MessageBox.Show("Jogador que sai é obrigatório.");
                return;
            }

            string entra = Interaction.InputBox("Jogador que entra?", "SUBSTITUIÇÃO", "");
            if (string.IsNullOrWhiteSpace(entra))
            {
                MessageBox.Show("Jogador que entra é obrigatório.");
                return;
            }

            _subs++;
            string ciName = $"sub{_subs}";

            var payload = new
            {
                tipo = "substituicao",
                minuto = minuto,
                sai = sai.Trim(),
                entra = entra.Trim(),
                equipaA = txtEquipaA.Text.Trim(),
                equipaB = txtEquipaB.Text.Trim(),
                timestamp = DateTime.UtcNow.ToString("o")
            };

            bool ok = await PublishEventAsync(ciName, payload);
            if (ok) MessageBox.Show($"Evento publicado: {ciName}");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // esconder tudo até iniciar jogo com sucesso
            SetJogoUIVisible(false);
        }

        private void SetJogoUIVisible(bool visible)
        {
            lblJogoAtual.Visible = visible;
            btnGolo.Visible = visible;
            btnCartao.Visible = visible;
            btnSub.Visible = visible;
            btnTerminar.Visible = visible;
        }

        #region Metodos 

        private async System.Threading.Tasks.Task<bool> CreateApplicationAsync(string appName)
        {
            var client = MakeClient();

            var body = new Dictionary<string, object>
            {
                { "resource-name", appName }
            };

            var req = new RestRequest("api/somiod", Method.Post);
            req.AddHeader("Content-Type", "application/json");
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            // 409 = já existe -> aceitável
            if ((int)res.StatusCode == 409) return true;

            MessageBox.Show($"Falha a criar Application.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        private async System.Threading.Tasks.Task<bool> CreateContainerAsync(string appName, string contName)
        {
            var client = MakeClient();

            var body = new Dictionary<string, object>
            {
                { "resource-name", contName }
            };

            // no teu middleware: POST /api/somiod/{appName}
            var req = new RestRequest($"api/somiod/{appName}", Method.Post);
            req.AddHeader("Content-Type", "application/json");
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            // 409 = já existe -> aceitável
            if ((int)res.StatusCode == 409) return true;

            MessageBox.Show($"Falha a criar Container.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        private async System.Threading.Tasks.Task<bool> PublishEventAsync(string ciName, object payloadObj)
        {
            var client = MakeClient();
            string payloadJson = JsonConvert.SerializeObject(payloadObj);

            var body = new Dictionary<string, object>
            {
                { "resource-name", ciName },
                { "content-type", "application/json" },
                { "content", payloadJson }
            };

            // no teu middleware: POST /api/somiod/{appName}/{contName}
            var req = new RestRequest($"api/somiod/{_appName}/{ContainerName}", Method.Post);
            req.AddHeader("Content-Type", "application/json");
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            MessageBox.Show($"Falha a publicar evento {ciName}.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        /*private async System.Threading.Tasks.Task<bool> DeleteApplicationAsync(string appName)
        {
            var client = MakeClient();

            var req = new RestRequest($"api/somiod/{appName}", Method.Delete);
            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            // 404 (já não existe) -> para reset UI, aceitável
            if ((int)res.StatusCode == 404) return true;

            MessageBox.Show($"Falha ao terminar jogo.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }*/

        private async System.Threading.Tasks.Task<bool> TerminateGameCascadeAsync(string appName)
        {
            try
            {
                var client = MakeClient();

                // 1) apagar content-instances do jogo (todas dentro dos containers da app)
                //    Fazemos DELETE direto na API: precisa de rota.
                //    Como tu não tens GET-all, vamos assumir o container "eventos" (o teu caso).
                //    Vamos apagar tudo o que conseguimos: golo1..N, cartao1..N, sub1..N
                //    (é simples e suficiente para a demo)

                // apagar golos
                for (int i = 1; i <= _golos; i++)
                    await client.ExecuteAsync(new RestRequest($"api/somiod/{appName}/{ContainerName}/golo{i}", Method.Delete));

                // apagar cartões
                for (int i = 1; i <= _cartoes; i++)
                    await client.ExecuteAsync(new RestRequest($"api/somiod/{appName}/{ContainerName}/cartao{i}", Method.Delete));

                // apagar substituições
                for (int i = 1; i <= _subs; i++)
                    await client.ExecuteAsync(new RestRequest($"api/somiod/{appName}/{ContainerName}/sub{i}", Method.Delete));

                // 2) apagar container
                await client.ExecuteAsync(new RestRequest($"api/somiod/{appName}/{ContainerName}", Method.Delete));

                // 3) apagar application
                var res = await client.ExecuteAsync(new RestRequest($"api/somiod/{appName}", Method.Delete));

                // 200 OK ou 404 já apagada -> ok
                if (res.IsSuccessful || (int)res.StatusCode == 404)
                    return true;

                MessageBox.Show($"Falha ao terminar jogo.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao terminar jogo: " + ex.Message);
                return false;
            }
        }

        private RestClient MakeClient()
        {
            var baseUrl = BaseUrl.EndsWith("/") ? BaseUrl : (BaseUrl + "/");
            return new RestClient(baseUrl);
        }


        private string BuildAppName(string equipaA, string equipaB)
        {
            string a = Slugify(equipaA);
            string b = Slugify(equipaB);
            return $"jogo-{a}-{b}";
        }

        // remove espaços/símbolos: só [a-z0-9-]
        private string Slugify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            string s = input.Trim().ToLowerInvariant();
            s = Regex.Replace(s, @"\s+", "-");
            s = Regex.Replace(s, @"[^a-z0-9\-]", "");
            s = Regex.Replace(s, @"\-{2,}", "-").Trim('-');

            return s;
        }

        #endregion
    }
}
