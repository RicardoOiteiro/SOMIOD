namespace AppSubscritor
{
    partial class FormJogo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormJogo));
            this.lblEquipaA = new System.Windows.Forms.Label();
            this.lblEquipaB = new System.Windows.Forms.Label();
            this.lblScoreA = new System.Windows.Forms.Label();
            this.lblScoreB = new System.Windows.Forms.Label();
            this.TextBoxEventos = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.listViewEvents = new System.Windows.Forms.ListView();
            this.Min = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Evento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Jogador = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnProcurarJogo = new System.Windows.Forms.Button();
            this.labelJogo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblEquipaA
            // 
            this.lblEquipaA.AutoSize = true;
            this.lblEquipaA.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipaA.Location = new System.Drawing.Point(199, 43);
            this.lblEquipaA.Name = "lblEquipaA";
            this.lblEquipaA.Size = new System.Drawing.Size(88, 18);
            this.lblEquipaA.TabIndex = 0;
            this.lblEquipaA.Text = "EQUIPA A";
            this.lblEquipaA.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblEquipaB
            // 
            this.lblEquipaB.AutoSize = true;
            this.lblEquipaB.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipaB.Location = new System.Drawing.Point(619, 43);
            this.lblEquipaB.Name = "lblEquipaB";
            this.lblEquipaB.Size = new System.Drawing.Size(88, 18);
            this.lblEquipaB.TabIndex = 1;
            this.lblEquipaB.Text = "EQUIPA B";
            // 
            // lblScoreA
            // 
            this.lblScoreA.AutoSize = true;
            this.lblScoreA.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreA.Location = new System.Drawing.Point(349, 43);
            this.lblScoreA.Name = "lblScoreA";
            this.lblScoreA.Size = new System.Drawing.Size(74, 78);
            this.lblScoreA.TabIndex = 2;
            this.lblScoreA.Text = "0";
            // 
            // lblScoreB
            // 
            this.lblScoreB.AutoSize = true;
            this.lblScoreB.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreB.Location = new System.Drawing.Point(520, 43);
            this.lblScoreB.Name = "lblScoreB";
            this.lblScoreB.Size = new System.Drawing.Size(74, 78);
            this.lblScoreB.TabIndex = 3;
            this.lblScoreB.Text = "0";
            // 
            // TextBoxEventos
            // 
            this.TextBoxEventos.Location = new System.Drawing.Point(879, 43);
            this.TextBoxEventos.Name = "TextBoxEventos";
            this.TextBoxEventos.Size = new System.Drawing.Size(422, 583);
            this.TextBoxEventos.TabIndex = 10;
            this.TextBoxEventos.Text = "";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Black;
            this.imageList1.Images.SetKeyName(0, "golo");
            this.imageList1.Images.SetKeyName(1, "subs");
            this.imageList1.Images.SetKeyName(2, "yellow_card");
            this.imageList1.Images.SetKeyName(3, "doubleyellow_card");
            this.imageList1.Images.SetKeyName(4, "red_card");
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(751, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listViewEvents
            // 
            this.listViewEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Evento,
            this.Jogador,
            this.Min});
            this.listViewEvents.FullRowSelect = true;
            this.listViewEvents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewEvents.HideSelection = false;
            this.listViewEvents.Location = new System.Drawing.Point(257, 153);
            this.listViewEvents.MultiSelect = false;
            this.listViewEvents.Name = "listViewEvents";
            this.listViewEvents.Size = new System.Drawing.Size(397, 430);
            this.listViewEvents.SmallImageList = this.imageList1;
            this.listViewEvents.TabIndex = 20;
            this.listViewEvents.UseCompatibleStateImageBehavior = false;
            this.listViewEvents.View = System.Windows.Forms.View.Details;
            // 
            // Min
            // 
            this.Min.DisplayIndex = 0;
            this.Min.Width = 100;
            // 
            // Evento
            // 
            this.Evento.DisplayIndex = 1;
            this.Evento.Width = 50;
            // 
            // Jogador
            // 
            this.Jogador.DisplayIndex = 2;
            this.Jogador.Width = 220;
            // 
            // btnProcurarJogo
            // 
            this.btnProcurarJogo.Location = new System.Drawing.Point(751, 193);
            this.btnProcurarJogo.Name = "btnProcurarJogo";
            this.btnProcurarJogo.Size = new System.Drawing.Size(84, 94);
            this.btnProcurarJogo.TabIndex = 21;
            this.btnProcurarJogo.Text = "Procurar jogo";
            this.btnProcurarJogo.UseVisualStyleBackColor = true;
            this.btnProcurarJogo.Click += new System.EventHandler(this.btnProcurarJogo_Click);
            // 
            // labelJogo
            // 
            this.labelJogo.AutoSize = true;
            this.labelJogo.Location = new System.Drawing.Point(421, 27);
            this.labelJogo.Name = "labelJogo";
            this.labelJogo.Size = new System.Drawing.Size(35, 13);
            this.labelJogo.TabIndex = 22;
            this.labelJogo.Text = "label1";
            // 
            // FormJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1321, 685);
            this.Controls.Add(this.labelJogo);
            this.Controls.Add(this.btnProcurarJogo);
            this.Controls.Add(this.listViewEvents);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TextBoxEventos);
            this.Controls.Add(this.lblScoreB);
            this.Controls.Add(this.lblScoreA);
            this.Controls.Add(this.lblEquipaB);
            this.Controls.Add(this.lblEquipaA);
            this.Name = "FormJogo";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEquipaA;
        private System.Windows.Forms.Label lblEquipaB;
        private System.Windows.Forms.Label lblScoreA;
        private System.Windows.Forms.Label lblScoreB;
        private System.Windows.Forms.RichTextBox TextBoxEventos;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewEvents;
        private System.Windows.Forms.ColumnHeader Min;
        private System.Windows.Forms.ColumnHeader Evento;
        private System.Windows.Forms.ColumnHeader Jogador;
        private System.Windows.Forms.Button btnProcurarJogo;
        private System.Windows.Forms.Label labelJogo;
    }
}

