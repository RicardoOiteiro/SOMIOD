using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace AppSubscritor
{
    public partial class Form1 : Form
    {
        MqttClient mClient = new MqttClient(IPAddress.Parse("54.36.178.49")); //OR use the broker hostname 
        string[] mStrTopicsInfo = { "api/somiod/stadiumApp/events", "estg_pl" };
        public Form1()
        {
            InitializeComponent();
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
        void AddEvento(int minuto, string tipo, string detalhes)
        {
            // tipo: "goal", "yellow", "red", "sub"
            var item = new ListViewItem($"{minuto}'");
            item.SubItems.Add(tipo);
            item.SubItems.Add(detalhes);

            item.ImageKey = tipo;         // usa as KEYS do ImageList
                                          // item.ImageIndex = 0;       // alternativa por índice

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

            // Garantir que está ligado ao ImageList
            listViewEvents.SmallImageList = imageList1;
            mClient.Connect(Guid.NewGuid().ToString());
            
        }

        private void lblMinutos1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEvento(15, "goal", "Pavlidis");
            AddEvento(29, "yellow_card", "João Neves");
            AddEvento(59, "red_card", "Pepe");
            AddEvento(70, "subs", "Sai: Rafa | Entra: Di María");
            AddEvento(85, "doubleyellow_card", "João Neves");
            ConnectAndSubscribe();



        }
        private void ConnectAndSubscribe()
        {
            if (!mClient.IsConnected)
            {
                MessageBox.Show("Error connecting to message broker...");
                return;
            }
            mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };//QoS 
            mClient.Subscribe(mStrTopicsInfo, qosLevels);
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            TextBoxEventos.BeginInvoke((MethodInvoker)delegate
            {
                //richTextBox1.AppendText("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic + Environment.NewLine);
                TextBoxEventos.AppendText("" + Encoding.UTF8.GetString(e.Message) + Environment.NewLine);
            });

        }
        private void lblEvento1A_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mClient.IsConnected)
            {
                mClient.Unsubscribe(mStrTopicsInfo); //Put this in a button to see notif! 
                mClient.Disconnect(); //Free process and process's resources 
            }
        }
    }
}
