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
            this.listViewEventsA = new System.Windows.Forms.ListView();
            this.Evento = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Jogador = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Min = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelJogo = new System.Windows.Forms.Label();
            this.listViewEventsB = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblEquipaA
            // 
            this.lblEquipaA.AutoSize = true;
            this.lblEquipaA.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipaA.Location = new System.Drawing.Point(173, 67);
            this.lblEquipaA.Name = "lblEquipaA";
            this.lblEquipaA.Size = new System.Drawing.Size(171, 38);
            this.lblEquipaA.TabIndex = 0;
            this.lblEquipaA.Text = "EQUIPA A";
            this.lblEquipaA.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblEquipaB
            // 
            this.lblEquipaB.AutoSize = true;
            this.lblEquipaB.Font = new System.Drawing.Font("Verdana", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEquipaB.Location = new System.Drawing.Point(563, 67);
            this.lblEquipaB.Name = "lblEquipaB";
            this.lblEquipaB.Size = new System.Drawing.Size(171, 38);
            this.lblEquipaB.TabIndex = 1;
            this.lblEquipaB.Text = "EQUIPA B";
            // 
            // lblScoreA
            // 
            this.lblScoreA.AutoSize = true;
            this.lblScoreA.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreA.Location = new System.Drawing.Point(373, 43);
            this.lblScoreA.Name = "lblScoreA";
            this.lblScoreA.Size = new System.Drawing.Size(74, 78);
            this.lblScoreA.TabIndex = 2;
            this.lblScoreA.Text = "0";
            // 
            // lblScoreB
            // 
            this.lblScoreB.AutoSize = true;
            this.lblScoreB.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreB.Location = new System.Drawing.Point(473, 43);
            this.lblScoreB.Name = "lblScoreB";
            this.lblScoreB.Size = new System.Drawing.Size(74, 78);
            this.lblScoreB.TabIndex = 3;
            this.lblScoreB.Text = "0";
            // 
            // TextBoxEventos
            // 
            this.TextBoxEventos.Location = new System.Drawing.Point(920, 27);
            this.TextBoxEventos.Name = "TextBoxEventos";
            this.TextBoxEventos.Size = new System.Drawing.Size(80, 229);
            this.TextBoxEventos.TabIndex = 10;
            this.TextBoxEventos.Text = "";
            this.TextBoxEventos.TextChanged += new System.EventHandler(this.TextBoxEventos_TextChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Black;
            this.imageList1.Images.SetKeyName(0, "golo");
            this.imageList1.Images.SetKeyName(1, "substituicao");
            this.imageList1.Images.SetKeyName(2, "amarelo");
            this.imageList1.Images.SetKeyName(3, "duploamarelo");
            this.imageList1.Images.SetKeyName(4, "vermelho");
            // 
            // listViewEventsA
            // 
            this.listViewEventsA.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Evento,
            this.Jogador,
            this.Min});
            this.listViewEventsA.FullRowSelect = true;
            this.listViewEventsA.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewEventsA.HideSelection = false;
            this.listViewEventsA.Location = new System.Drawing.Point(44, 158);
            this.listViewEventsA.MultiSelect = false;
            this.listViewEventsA.Name = "listViewEventsA";
            this.listViewEventsA.Size = new System.Drawing.Size(375, 421);
            this.listViewEventsA.SmallImageList = this.imageList1;
            this.listViewEventsA.TabIndex = 20;
            this.listViewEventsA.UseCompatibleStateImageBehavior = false;
            this.listViewEventsA.View = System.Windows.Forms.View.Details;
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
            // Min
            // 
            this.Min.DisplayIndex = 0;
            this.Min.Width = 100;
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
            // listViewEventsB
            // 
            this.listViewEventsB.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewEventsB.FullRowSelect = true;
            this.listViewEventsB.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewEventsB.HideSelection = false;
            this.listViewEventsB.Location = new System.Drawing.Point(486, 158);
            this.listViewEventsB.MultiSelect = false;
            this.listViewEventsB.Name = "listViewEventsB";
            this.listViewEventsB.Size = new System.Drawing.Size(375, 421);
            this.listViewEventsB.SmallImageList = this.imageList1;
            this.listViewEventsB.TabIndex = 23;
            this.listViewEventsB.UseCompatibleStateImageBehavior = false;
            this.listViewEventsB.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 1;
            this.columnHeader1.Width = 50;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 2;
            this.columnHeader2.Width = 220;
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 0;
            this.columnHeader3.Width = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(442, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 42);
            this.label1.TabIndex = 24;
            this.label1.Text = "-";
            // 
            // FormJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 591);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewEventsB);
            this.Controls.Add(this.labelJogo);
            this.Controls.Add(this.listViewEventsA);
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
        private System.Windows.Forms.ListView listViewEventsA;
        private System.Windows.Forms.ColumnHeader Min;
        private System.Windows.Forms.ColumnHeader Evento;
        private System.Windows.Forms.ColumnHeader Jogador;
        private System.Windows.Forms.Label labelJogo;
        private System.Windows.Forms.ListView listViewEventsB;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label1;
    }
}

