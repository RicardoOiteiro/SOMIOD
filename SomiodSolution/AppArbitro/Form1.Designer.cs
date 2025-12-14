namespace AppArbitro
{
    partial class Form1
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
            this.txtEquipaA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEquipaB = new System.Windows.Forms.TextBox();
            this.btnIniciarJogo = new System.Windows.Forms.Button();
            this.btnGolo = new System.Windows.Forms.Button();
            this.btnCartao = new System.Windows.Forms.Button();
            this.btnSub = new System.Windows.Forms.Button();
            this.btnTerminar = new System.Windows.Forms.Button();
            this.lblJogoAtual = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtEquipaA
            // 
            this.txtEquipaA.Location = new System.Drawing.Point(179, 126);
            this.txtEquipaA.Name = "txtEquipaA";
            this.txtEquipaA.Size = new System.Drawing.Size(100, 20);
            this.txtEquipaA.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(116, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "EQUIPA A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "EQUIPA B";
            // 
            // txtEquipaB
            // 
            this.txtEquipaB.Location = new System.Drawing.Point(179, 165);
            this.txtEquipaB.Name = "txtEquipaB";
            this.txtEquipaB.Size = new System.Drawing.Size(100, 20);
            this.txtEquipaB.TabIndex = 3;
            // 
            // btnIniciarJogo
            // 
            this.btnIniciarJogo.Location = new System.Drawing.Point(358, 109);
            this.btnIniciarJogo.Name = "btnIniciarJogo";
            this.btnIniciarJogo.Size = new System.Drawing.Size(162, 96);
            this.btnIniciarJogo.TabIndex = 4;
            this.btnIniciarJogo.Text = "INICIAR JOGO";
            this.btnIniciarJogo.UseVisualStyleBackColor = true;
            this.btnIniciarJogo.Click += new System.EventHandler(this.btnIniciarJogo_Click);
            // 
            // btnGolo
            // 
            this.btnGolo.Location = new System.Drawing.Point(135, 236);
            this.btnGolo.Name = "btnGolo";
            this.btnGolo.Size = new System.Drawing.Size(144, 53);
            this.btnGolo.TabIndex = 5;
            this.btnGolo.Text = "btnGolo";
            this.btnGolo.UseVisualStyleBackColor = true;
            this.btnGolo.Click += new System.EventHandler(this.btnGolo_Click);
            // 
            // btnCartao
            // 
            this.btnCartao.Location = new System.Drawing.Point(135, 295);
            this.btnCartao.Name = "btnCartao";
            this.btnCartao.Size = new System.Drawing.Size(144, 53);
            this.btnCartao.TabIndex = 6;
            this.btnCartao.Text = "btnCartao";
            this.btnCartao.UseVisualStyleBackColor = true;
            this.btnCartao.Click += new System.EventHandler(this.btnCartao_Click);
            // 
            // btnSub
            // 
            this.btnSub.Location = new System.Drawing.Point(135, 354);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(144, 53);
            this.btnSub.TabIndex = 7;
            this.btnSub.Text = "btnSub";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnTerminar
            // 
            this.btnTerminar.Location = new System.Drawing.Point(487, 368);
            this.btnTerminar.Name = "btnTerminar";
            this.btnTerminar.Size = new System.Drawing.Size(144, 53);
            this.btnTerminar.TabIndex = 8;
            this.btnTerminar.Text = "btnTerminar";
            this.btnTerminar.UseVisualStyleBackColor = true;
            this.btnTerminar.Click += new System.EventHandler(this.btnTerminar_Click);
            // 
            // lblJogoAtual
            // 
            this.lblJogoAtual.AutoSize = true;
            this.lblJogoAtual.Location = new System.Drawing.Point(425, 256);
            this.lblJogoAtual.Name = "lblJogoAtual";
            this.lblJogoAtual.Size = new System.Drawing.Size(35, 13);
            this.lblJogoAtual.TabIndex = 9;
            this.lblJogoAtual.Text = "label3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblJogoAtual);
            this.Controls.Add(this.btnTerminar);
            this.Controls.Add(this.btnSub);
            this.Controls.Add(this.btnCartao);
            this.Controls.Add(this.btnGolo);
            this.Controls.Add(this.btnIniciarJogo);
            this.Controls.Add(this.txtEquipaB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtEquipaA);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEquipaA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEquipaB;
        private System.Windows.Forms.Button btnIniciarJogo;
        private System.Windows.Forms.Button btnGolo;
        private System.Windows.Forms.Button btnCartao;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.Button btnTerminar;
        private System.Windows.Forms.Label lblJogoAtual;
    }
}

