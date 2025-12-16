namespace AppSubscritor
{
    partial class FormEscolherJogo
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
            this.btnProcurarJogo = new System.Windows.Forms.Button();
            this.btnVerJogo = new System.Windows.Forms.Button();
            this.listBoxJogos = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labelSelectedGame = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnProcurarJogo
            // 
            this.btnProcurarJogo.Location = new System.Drawing.Point(60, 23);
            this.btnProcurarJogo.Name = "btnProcurarJogo";
            this.btnProcurarJogo.Size = new System.Drawing.Size(194, 60);
            this.btnProcurarJogo.TabIndex = 0;
            this.btnProcurarJogo.Text = "Procurar Jogo";
            this.btnProcurarJogo.UseVisualStyleBackColor = true;
            this.btnProcurarJogo.Click += new System.EventHandler(this.btnProcurarJogo_Click);
            // 
            // btnVerJogo
            // 
            this.btnVerJogo.Location = new System.Drawing.Point(82, 419);
            this.btnVerJogo.Name = "btnVerJogo";
            this.btnVerJogo.Size = new System.Drawing.Size(172, 45);
            this.btnVerJogo.TabIndex = 2;
            this.btnVerJogo.Text = "Ver Jogo";
            this.btnVerJogo.UseVisualStyleBackColor = true;
            this.btnVerJogo.Click += new System.EventHandler(this.btnVerJogo_Click);
            // 
            // listBoxJogos
            // 
            this.listBoxJogos.FormattingEnabled = true;
            this.listBoxJogos.Location = new System.Drawing.Point(12, 105);
            this.listBoxJogos.Name = "listBoxJogos";
            this.listBoxJogos.Size = new System.Drawing.Size(264, 225);
            this.listBoxJogos.TabIndex = 3;
            this.listBoxJogos.SelectedIndexChanged += new System.EventHandler(this.listBoxJogos_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(34, 351);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Jogo Selecionado";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // labelSelectedGame
            // 
            this.labelSelectedGame.AutoSize = true;
            this.labelSelectedGame.ForeColor = System.Drawing.Color.Blue;
            this.labelSelectedGame.Location = new System.Drawing.Point(148, 351);
            this.labelSelectedGame.Name = "labelSelectedGame";
            this.labelSelectedGame.Size = new System.Drawing.Size(135, 13);
            this.labelSelectedGame.TabIndex = 5;
            this.labelSelectedGame.Text = "Nenhum Jogo Selecionado";
            // 
            // FormEscolherJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 476);
            this.Controls.Add(this.labelSelectedGame);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBoxJogos);
            this.Controls.Add(this.btnVerJogo);
            this.Controls.Add(this.btnProcurarJogo);
            this.Name = "FormEscolherJogo";
            this.Text = "FormEscolherJogo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcurarJogo;
        private System.Windows.Forms.Button btnVerJogo;
        private System.Windows.Forms.ListBox listBoxJogos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelSelectedGame;
    }
}