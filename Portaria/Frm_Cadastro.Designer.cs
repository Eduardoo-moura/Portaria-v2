namespace Portaria
{
    partial class Frm_Cadastro
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Btn_Veiculo = new System.Windows.Forms.Button();
            this.Btn_Pessoas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Btn_Veiculo
            // 
            this.Btn_Veiculo.Location = new System.Drawing.Point(35, 29);
            this.Btn_Veiculo.Name = "Btn_Veiculo";
            this.Btn_Veiculo.Size = new System.Drawing.Size(127, 110);
            this.Btn_Veiculo.TabIndex = 0;
            this.Btn_Veiculo.Text = "VEICULO";
            this.Btn_Veiculo.UseVisualStyleBackColor = true;
            this.Btn_Veiculo.Click += new System.EventHandler(this.Btn_Veiculo_Click);
            // 
            // Btn_Pessoas
            // 
            this.Btn_Pessoas.Location = new System.Drawing.Point(228, 29);
            this.Btn_Pessoas.Name = "Btn_Pessoas";
            this.Btn_Pessoas.Size = new System.Drawing.Size(127, 110);
            this.Btn_Pessoas.TabIndex = 1;
            this.Btn_Pessoas.Text = "PESSOAS";
            this.Btn_Pessoas.UseVisualStyleBackColor = true;
            // 
            // Frm_Cadastro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 222);
            this.Controls.Add(this.Btn_Pessoas);
            this.Controls.Add(this.Btn_Veiculo);
            this.Name = "Frm_Cadastro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CADASTRO";
            this.Load += new System.EventHandler(this.CADASTRO_Load);
            this.Click += new System.EventHandler(this.Btn_Veiculo_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Btn_Veiculo;
        private System.Windows.Forms.Button Btn_Pessoas;
    }
}

