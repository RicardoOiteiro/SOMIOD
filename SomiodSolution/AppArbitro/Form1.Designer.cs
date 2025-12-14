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
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtEquipaA
            // 
            this.txtEquipaA.Location = new System.Drawing.Point(158, 81);
            this.txtEquipaA.Name = "txtEquipaA";
            this.txtEquipaA.Size = new System.Drawing.Size(100, 20);
            this.txtEquipaA.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(69, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Equipa A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Palatino Linotype", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(69, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Equipa B";
            // 
            // txtEquipaB
            // 
            this.txtEquipaB.Location = new System.Drawing.Point(158, 135);
            this.txtEquipaB.Name = "txtEquipaB";
            this.txtEquipaB.Size = new System.Drawing.Size(100, 20);
            this.txtEquipaB.TabIndex = 3;
            // 
            // btnIniciarJogo
            // 
            this.btnIniciarJogo.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnIniciarJogo.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciarJogo.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnIniciarJogo.Location = new System.Drawing.Point(298, 81);
            this.btnIniciarJogo.Name = "btnIniciarJogo";
            this.btnIniciarJogo.Size = new System.Drawing.Size(154, 74);
            this.btnIniciarJogo.TabIndex = 4;
            this.btnIniciarJogo.Text = "INICIAR JOGO";
            this.btnIniciarJogo.UseVisualStyleBackColor = false;
            this.btnIniciarJogo.Click += new System.EventHandler(this.btnIniciarJogo_Click);
            // 
            // btnGolo
            // 
            this.btnGolo.Location = new System.Drawing.Point(73, 158);
            this.btnGolo.Name = "btnGolo";
            this.btnGolo.Size = new System.Drawing.Size(144, 53);
            this.btnGolo.TabIndex = 5;
            this.btnGolo.Text = "⚽ GOLO";
            this.btnGolo.UseVisualStyleBackColor = true;
            this.btnGolo.Click += new System.EventHandler(this.btnGolo_Click);
            // 
            // btnCartao
            // 
            this.btnCartao.Location = new System.Drawing.Point(73, 256);
            this.btnCartao.Name = "btnCartao";
            this.btnCartao.Size = new System.Drawing.Size(144, 53);
            this.btnCartao.TabIndex = 6;
            this.btnCartao.Text = "🟨/🟥 CARTÃO";
            this.btnCartao.UseVisualStyleBackColor = true;
            this.btnCartao.Click += new System.EventHandler(this.btnCartao_Click);
            // 
            // btnSub
            // 
            this.btnSub.Location = new System.Drawing.Point(73, 354);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(144, 53);
            this.btnSub.TabIndex = 7;
            this.btnSub.Text = "🔁 SUBSTITUIÇÃO";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnTerminar
            // 
            this.btnTerminar.BackColor = System.Drawing.Color.Red;
            this.btnTerminar.Font = new System.Drawing.Font("Palatino Linotype", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTerminar.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.btnTerminar.Location = new System.Drawing.Point(296, 297);
            this.btnTerminar.Name = "btnTerminar";
            this.btnTerminar.Size = new System.Drawing.Size(156, 63);
            this.btnTerminar.TabIndex = 8;
            this.btnTerminar.Text = "TERMINAR JOGO";
            this.btnTerminar.UseVisualStyleBackColor = false;
            this.btnTerminar.Click += new System.EventHandler(this.btnTerminar_Click);
            // 
            // lblJogoAtual
            // 
            this.lblJogoAtual.AutoSize = true;
            this.lblJogoAtual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJogoAtual.Location = new System.Drawing.Point(254, 109);
            this.lblJogoAtual.Name = "lblJogoAtual";
            this.lblJogoAtual.Size = new System.Drawing.Size(46, 18);
            this.lblJogoAtual.TabIndex = 9;
            this.lblJogoAtual.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(109, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 26);
            this.label3.TabIndex = 10;
            this.label3.Text = "JOGO ATUAL:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 490);
            this.Controls.Add(this.label3);
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
        private System.Windows.Forms.Label label3;
    }
}

