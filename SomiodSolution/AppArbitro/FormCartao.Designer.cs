namespace AppArbitro
{
    partial class FormCartao
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
            this.numMinuto = new System.Windows.Forms.NumericUpDown();
            this.txtJogador = new System.Windows.Forms.TextBox();
            this.cbEquipa = new System.Windows.Forms.ComboBox();
            this.cbTipoCartao = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numMinuto)).BeginInit();
            this.SuspendLayout();
            // 
            // numMinuto
            // 
            this.numMinuto.Location = new System.Drawing.Point(89, 9);
            this.numMinuto.Name = "numMinuto";
            this.numMinuto.Size = new System.Drawing.Size(120, 20);
            this.numMinuto.TabIndex = 0;
            // 
            // txtJogador
            // 
            this.txtJogador.Location = new System.Drawing.Point(109, 52);
            this.txtJogador.Name = "txtJogador";
            this.txtJogador.Size = new System.Drawing.Size(100, 20);
            this.txtJogador.TabIndex = 1;
            // 
            // cbEquipa
            // 
            this.cbEquipa.FormattingEnabled = true;
            this.cbEquipa.Location = new System.Drawing.Point(88, 95);
            this.cbEquipa.Name = "cbEquipa";
            this.cbEquipa.Size = new System.Drawing.Size(121, 21);
            this.cbEquipa.TabIndex = 2;
            // 
            // cbTipoCartao
            // 
            this.cbTipoCartao.FormattingEnabled = true;
            this.cbTipoCartao.Location = new System.Drawing.Point(88, 139);
            this.cbTipoCartao.Name = "cbTipoCartao";
            this.cbTipoCartao.Size = new System.Drawing.Size(121, 21);
            this.cbTipoCartao.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnOk.Location = new System.Drawing.Point(32, 182);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Submeter";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnCancelar.Location = new System.Drawing.Point(134, 182);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Minuto:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Equipa:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Jogador:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 142);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Cartão:";
            // 
            // FormCartao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 216);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbTipoCartao);
            this.Controls.Add(this.cbEquipa);
            this.Controls.Add(this.txtJogador);
            this.Controls.Add(this.numMinuto);
            this.Name = "FormCartao";
            this.Text = "FormCartao";
            ((System.ComponentModel.ISupportInitialize)(this.numMinuto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numMinuto;
        private System.Windows.Forms.TextBox txtJogador;
        private System.Windows.Forms.ComboBox cbEquipa;
        private System.Windows.Forms.ComboBox cbTipoCartao;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}