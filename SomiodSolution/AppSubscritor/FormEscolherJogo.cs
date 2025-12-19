using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppSubscritor
{
    public partial class FormEscolherJogo : Form
    {
        private const string baseURL = "http://localhost:52047/";
        public FormEscolherJogo()
        {
            InitializeComponent();
        }

        private void btnProcurarJogo_Click(object sender, EventArgs e)
        {
            listBoxJogos.Items.Clear();

            var client = new RestSharp.RestClient(baseURL);
            var request = new RestSharp.RestRequest("api/somiod", RestSharp.Method.Get);

            request.AddHeader("somiod-discovery", "application");
            request.RequestFormat = RestSharp.DataFormat.Json;

            var response = client.Execute<List<string>>(request);

            if (response.Data == null)
            {
                MessageBox.Show("Nenhum jogo encontrado.");
                return;
            }

            foreach (var appPath in response.Data)
            {
                // appPath vem tipo "/api/somiod/appName"
                var appName = appPath.Split('/').Last();

                listBoxJogos.Items.Add($"{appName}{Environment.NewLine}");
            }
        }

        private void btnVerJogo_Click(object sender, EventArgs e)
        {
         
            String Jogo = labelSelectedGame.Text;
            var formJogo = new FormJogo(Jogo);
            formJogo.Show();

            
            this.Hide();
        }
       

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBoxJogos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxJogos.SelectedItem != null)
            {
                labelSelectedGame.Text = listBoxJogos.SelectedItem.ToString();
                btnVerJogo.Visible = true;
            }
        }

        private void FormEscolherJogo_Load(object sender, EventArgs e)
        {
            btnVerJogo.Visible = false;
        }
    }

}
