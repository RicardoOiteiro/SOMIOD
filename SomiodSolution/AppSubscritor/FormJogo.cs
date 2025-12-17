using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text.Json;

namespace AppSubscritor
{
    public partial class FormJogo : Form
    {
        MqttClient mClient = new MqttClient(IPAddress.Parse("54.36.178.49")); //OR use the broker hostname 
        //string[] mStrTopicsInfo = { "api/somiod/stadiumApp/events", "estg_pl" };
        private readonly string _appName;
        private string[] topics;
        private string[] cartoes;
        private int ncartoes = 0;
        private string equipaCasa, equipaFora;


        public FormJogo(string appName)
        {
            InitializeComponent();
            _appName = appName.Trim();
            topics = new[]
            {
                $"api/somiod/{_appName}/golos",
                $"api/somiod/{_appName}/cartoes",
                $"api/somiod/{_appName}/substituicoes",
            };
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblMinutos4_Click(object sender, EventArgs e)
        {

        }

        private void lblMinutos5_Click(object sender, EventArgs e)
        {

        }

        private void lblEventos_Click(object sender, EventArgs e)
        {

        }
        void AddEvento(string tipo,int minuto, string jogador,string equipa,string tipocartao)
        {

            if(tipocartao != null)
            {
                 
                if(String.Equals(tipocartao.Trim(), "amarelo"))
                {
                    tipo = tipocartao.ToLower();

                    for (int i = 0; i < ncartoes; i++)
                    {
                        if (String.Equals(cartoes[i].Trim(), jogador.Trim()))
                        {
                            tipo = "duploamarelo";
                            break;
                        }
                    }
                }
                else if(String.Equals(tipocartao.Trim(), "vermelho"))
                {
                    tipo = tipocartao.ToLower();
                }
                cartoes[ncartoes] = jogador.Trim();
                ncartoes += 1;

            }
            equipa = equipa.ToLower();
           



            var item = new ListViewItem("");          // 1ª coluna "Evento" (texto vazio)
            


            item.ImageKey = tipo;         // usa as KEYS do ImageList
                                          // item.ImageIndex = 0;       // alternativa por índice

            

            item.SubItems.Add(jogador);               // coluna "Jogador"
            item.SubItems.Add(minuto.ToString());     // coluna "Min"
            if(String.Equals(equipa.Trim(), equipaCasa.Trim()))
            {
                listViewEventsA.Items.Add(item);
                // auto-scroll para o último
                listViewEventsA.EnsureVisible(listViewEventsA.Items.Count - 1);   // coluna "Equipa"
                if(tipo == "golo")
                {
                    int golos = int.Parse(lblScoreA.Text);
                    golos += 1;
                    lblScoreA.Text = golos.ToString();
                }
            }
            else
            {
                listViewEventsB.Items.Add(item);
                // auto-scroll para o último
                listViewEventsB.EnsureVisible(listViewEventsB.Items.Count - 1);   // coluna "Equipa"
                if (tipo == "golo")
                {
                    int golos = int.Parse(lblScoreB.Text);
                    golos += 1;
                    lblScoreB.Text = golos.ToString();
                }
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listViewEventsA.View = View.Details;
            listViewEventsA.FullRowSelect = true;

            // Se ainda não criaste colunas no Designer, cria aqui:
            if (listViewEventsA.Columns.Count == 0)
            {
                listViewEventsA.Columns.Add("Min", 100);
                listViewEventsA.Columns.Add("Evento", 0);
                listViewEventsA.Columns.Add("Detalhes", 250);
            }
            listViewEventsB.View = View.Details;
            listViewEventsB.FullRowSelect = true;

            // Se ainda não criaste colunas no Designer, cria aqui:
            if (listViewEventsB.Columns.Count == 0)
            {
                listViewEventsB.Columns.Add("Min", 100);
                listViewEventsB.Columns.Add("Evento", 0);
                listViewEventsB.Columns.Add("Detalhes", 250);
            }
            labelJogo.Text = _appName;

            

            // Garantir que está ligado ao ImageList
            listViewEventsA.SmallImageList = imageList1;
            listViewEventsB.SmallImageList = imageList1;
            mClient.Connect(Guid.NewGuid().ToString());
            TextBoxEventos.Visible = false;
            labelJogo.Visible = false;
            cartoes = new string[200];
            ConnectAndSubscribe();
            StartMatch(_appName);


        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            TextBoxEventos.BeginInvoke((MethodInvoker)delegate
            {
                //richTextBox1.AppendText("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic + Environment.NewLine);
                TextBoxEventos.AppendText("" + Encoding.UTF8.GetString(e.Message) + Environment.NewLine);
                string payload = Encoding.UTF8.GetString(e.Message);
                using (JsonDocument doc = JsonDocument.Parse(payload))
                {
                    string content = doc.RootElement
                                        .GetProperty("content")
                                        .GetString();

                    // JSON interior
                    using (JsonDocument contentDoc = JsonDocument.Parse(content))
                    {
                        string tipo = contentDoc.RootElement.GetProperty("tipo").GetString();
                        int minuto = contentDoc.RootElement.GetProperty("minuto").GetInt32();
                        if (string.Equals(tipo.Trim(), "substituicao"))
                        {
                            string jogadorEntrou = contentDoc.RootElement.GetProperty("entra").GetString();
                            string jogadorSaiu = contentDoc.RootElement.GetProperty("sai").GetString();
                            string equipa = contentDoc.RootElement.GetProperty("equipa").GetString();
                            AddEvento(tipo, minuto, $"{jogadorSaiu} -> {jogadorEntrou}", equipa,null);
                        }
                        else if (string.Equals(tipo.Trim(), "cartao"))
                        {
                            string jogador = contentDoc.RootElement.GetProperty("jogador").GetString();
                            string equipa = contentDoc.RootElement.GetProperty("equipa").GetString();
                            string cartao = contentDoc.RootElement.GetProperty("cartao").GetString();
                            AddEvento(tipo, minuto, jogador, equipa, cartao);
                        }
                        else
                        {
                            string jogador = contentDoc.RootElement.GetProperty("jogador").GetString();
                            string equipa = contentDoc.RootElement.GetProperty("equipa").GetString();
                            AddEvento(tipo, minuto, jogador, equipa,null);
                        }
                            
                    }
                }

            });

        }

        private void StartMatch(string appName)
        {


            string[] equipas = appName.Split('-');

            equipaCasa = equipas[1];   
            equipaFora = equipas[2];   
            lblEquipaA.Text = equipaCasa;
            lblEquipaB.Text = equipaFora;
            


        }
        private void lblMinutos1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            


        }
        private void ConnectAndSubscribe()
        {
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker...");
                return;
            }
            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
        
            mClient.Subscribe(topics, new[]
            {
                MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE,
                MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE
             });
        }

        private void lblEvento1A_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mClient.IsConnected)
            {
                mClient.Unsubscribe(topics); //Put this in a button to see notif! 
                mClient.Disconnect(); //Free process and process's resources 
            }
        }

        private void btnProcurarJogo_Click(object sender, EventArgs e)
        {
            
        }

        private void TextBoxEventos_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
