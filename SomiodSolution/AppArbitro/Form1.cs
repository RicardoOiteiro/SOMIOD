using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using AppArbitro.Dtos;

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

            //criar application
            bool okApp = await CreateApplicationAsync(_appName);
            if (!okApp)
            {
                _appName = null;
                btnIniciarJogo.Enabled = true;
                return;
            }

            //criar container "eventos"
            bool okCont = await CreateContainerAsync(_appName, ContainerName);
            if (!okCont)
            {
                MessageBox.Show("Application criada, mas falhou a criação do container 'eventos'.");
                _appName = null;
                btnIniciarJogo.Enabled = true;
                return;
            }

            // reset
            _golos = 0;
            _cartoes = 0;
            _subs = 0;

            // UI
            lblJogoAtual.Text = $" {_appName}";
            SetJogoUIVisible(true);

            SetStartUIVisible(false);
            SetJogoUIVisible(true);

            MessageBox.Show("Inicio do Jogo!");
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

                SetStartUIVisible(true);
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

            using (var f = new FormGolo(txtEquipaA.Text.Trim(), txtEquipaB.Text.Trim()))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;

                _golos++;
                string ciName = $"golo{_golos}";

                var payload = new
                {
                    tipo = "golo",
                    minuto = f.Data.Minuto,
                    jogador = f.Data.Jogador,
                    equipa = f.Data.Equipa
                };

                bool ok = await PublishEventAsync(ciName, payload);
                if (ok) MessageBox.Show($"Golo assinalado: {ciName}");
            }
        }

        private async void btnCartao_Click(object sender, EventArgs e)
        {
            if (_appName == null) return;

            string equipaA = txtEquipaA.Text.Trim();
            string equipaB = txtEquipaB.Text.Trim();

            using (var f = new FormCartao(equipaA, equipaB))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;

                _cartoes++;
                string ciName = $"cartao{_cartoes}";

                var payload = new
                {
                    tipo = "cartao",
                    minuto = f.Result.Minuto,
                    jogador = f.Result.Jogador,
                    cartao = f.Result.TipoCartao,
                    equipa = f.Result.Equipa
                };

                bool ok = await PublishEventAsync(ciName, payload);
                if (ok) MessageBox.Show($"Cartão atribuído: {ciName}");
            }
        }

        private async void btnSub_Click(object sender, EventArgs e)
        {
            if (_appName == null) return;

            string equipaA = txtEquipaA.Text.Trim();
            string equipaB = txtEquipaB.Text.Trim();

            using (var f = new FormSubstituicao(equipaA, equipaB))
            {
                if (f.ShowDialog(this) != DialogResult.OK) return;

                _subs++;
                string ciName = $"sub{_subs}";

                var payload = new
                {
                    tipo = "substituicao",
                    minuto = f.Result.Minuto,
                    sai = f.Result.Sai,
                    entra = f.Result.Entra,
                    equipa = f.Result.Equipa
                };

                bool ok = await PublishEventAsync(ciName, payload);
                if (ok) MessageBox.Show($"Substituição realizada: {ciName}");
            }
        }

        private async void btnEditarEquipas_Click(object sender, EventArgs e)
        {
            if (_appName == null)
            {
                MessageBox.Show("Nenhum jogo iniciado.");
                return;
            }

            using (var f = new FormEditarEquipas(
                txtEquipaA.Text.Trim(),
                txtEquipaB.Text.Trim()))
            {
                if (f.ShowDialog(this) != DialogResult.OK)
                    return;

                string novoAppName = BuildAppName(f.EquipaA, f.EquipaB);

                if (string.Equals(novoAppName, _appName, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("O nome do jogo não mudou.");
                    return;
                }

                btnEditarEquipas.Enabled = false;

                bool ok = await PutApplicationAsync(_appName, novoAppName);

                if (ok)
                {
                    _appName = novoAppName;

                    txtEquipaA.Text = f.EquipaA;
                    txtEquipaB.Text = f.EquipaB;
                    lblJogoAtual.Text = $" {_appName}";

                    MessageBox.Show("Equipas atualizadas com sucesso!");
                }

                btnEditarEquipas.Enabled = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetStartUIVisible(true);
            // esconder tudo até iniciar jogo com sucesso
            SetJogoUIVisible(false);
        }

        private void SetJogoUIVisible(bool visible)
        {
            label3.Visible = visible;
            lblJogoAtual.Visible = visible;
            btnGolo.Visible = visible;
            btnCartao.Visible = visible;
            btnSub.Visible = visible;
            btnTerminar.Visible = visible;
            btnEditarEquipas.Visible = visible;
        }

        private void SetStartUIVisible(bool visible)
        {
            label1.Visible = visible;
            label2.Visible = visible;
            txtEquipaA.Visible = visible;
            txtEquipaB.Visible = visible;
            btnIniciarJogo.Visible = visible;
        }

        #region Metodos 


        private async System.Threading.Tasks.Task<bool> CreateApplicationAsync(string appName)
        {
            var client = MakeClient();

            var body = new ApplicationDto
            {
                ResourceName = appName
            };

            var req = new RestRequest("api/somiod", Method.Post);
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            // 409 = já existe 
            if ((int)res.StatusCode == 409) return true;

            MessageBox.Show($"Falha a criar Application.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        private async System.Threading.Tasks.Task<bool> CreateContainerAsync(string appName, string contName)
        {
            var client = MakeClient();

            var body = new ContainerDto
            {
                ResourceName = contName
            };

            var req = new RestRequest($"api/somiod/{appName}", Method.Post);
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            // 409 = já existe 
            if ((int)res.StatusCode == 409) return true;

            MessageBox.Show($"Falha a criar Container.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        private async System.Threading.Tasks.Task<bool> PublishEventAsync(string ciName, object payloadObj)
        {
            var client = MakeClient();
            string payloadJson = JsonConvert.SerializeObject(payloadObj);

            var body = new ContentInstanceDto
            {
                ResourceName = ciName,
                ContentType = "application/json",
                Content = payloadJson
            };

            var req = new RestRequest($"api/somiod/{_appName}/{ContainerName}", Method.Post);
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            MessageBox.Show($"Falha a publicar evento {ciName}.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        private async System.Threading.Tasks.Task<bool> TerminateGameCascadeAsync(string appName)
        {
            try
            {
                var client = MakeClient();


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

        private async System.Threading.Tasks.Task<bool> PutApplicationAsync(string oldAppName, string newAppName)
        {
            var client = MakeClient();

            var body = new ApplicationDto
            {
                ResourceName = newAppName
            };

            var req = new RestRequest($"api/somiod/{oldAppName}", Method.Put);
            req.AddJsonBody(body);

            var res = await client.ExecuteAsync(req);

            if (res.IsSuccessful) return true;

            MessageBox.Show($"Falha a atualizar Application.\nHTTP {(int)res.StatusCode} {res.StatusCode}\n{res.Content}");
            return false;
        }

        #endregion

    }
}
