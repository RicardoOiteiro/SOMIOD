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


        public FormJogo(string appName)
        {
            InitializeComponent();
            _appName = appName.Trim();
            topics = new[]
            {
                $"api/somiod/{_appName}/golos",
                $"api/somiod/{_appName}/cartoes",
                $"api/somiod/{_appName}/substituicoes",
                $"api/somiod/{_appName}/eventos"
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
        void AddEvento(string tipo,int minuto, string jogador,string equipa)
        {




            // tipo: "goal", "yellow", "red", "sub"
            var item = new ListViewItem("");          // 1ª coluna "Evento" (texto vazio)
            


            item.ImageKey = tipo;         // usa as KEYS do ImageList
                                          // item.ImageIndex = 0;       // alternativa por índice

            

            item.SubItems.Add(jogador);               // coluna "Jogador"
            item.SubItems.Add(minuto.ToString());     // coluna "Min"

            listViewEvents.Items.Add(item);
            // auto-scroll para o último
            listViewEvents.EnsureVisible(listViewEvents.Items.Count - 1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listViewEvents.View = View.Details;
            listViewEvents.FullRowSelect = true;

            // Se ainda não criaste colunas no Designer, cria aqui:
            if (listViewEvents.Columns.Count == 0)
            {
                listViewEvents.Columns.Add("Min", 100);
                listViewEvents.Columns.Add("Evento", 0);
                listViewEvents.Columns.Add("Detalhes", 250);
            }
            labelJogo.Text = _appName;

            

            // Garantir que está ligado ao ImageList
            listViewEvents.SmallImageList = imageList1;
            mClient.Connect(Guid.NewGuid().ToString());
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
                        string jogador = contentDoc.RootElement.GetProperty("jogador").GetString();
                        string equipa = contentDoc.RootElement.GetProperty("equipa").GetString();
                        AddEvento(tipo, minuto, jogador, equipa);
                    }
                }

            });

        }

        private void StartMatch(string appName)
        {


            string[] equipas = appName.Split('-');

            string equipaCasa = equipas[1];   
            string equipaFora = equipas[2];   
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
            
            MessageBox.Show(topics[0]);


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
        

    }
}
