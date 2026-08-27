namespace Portaria
{
    partial class Frm_Mercadoria
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        private void InitializeComponent()
        {
            this.menu_mercadoria = new System.Windows.Forms.MenuStrip();
            this.Strip_relatorio = new System.Windows.Forms.ToolStripMenuItem();
            this.Relatorio_personalizado = new System.Windows.Forms.ToolStripMenuItem();
            this.grp_dados = new System.Windows.Forms.GroupBox();
            this.lbl_destinatario = new System.Windows.Forms.Label();
            this.txt_Destinatario = new System.Windows.Forms.TextBox();
            this.lbl_empresa = new System.Windows.Forms.Label();
            this.txt_Empresa = new System.Windows.Forms.TextBox();
            this.lbl_entregador = new System.Windows.Forms.Label();
            this.txt_Entregador = new System.Windows.Forms.TextBox();
            this.lbl_recebedor = new System.Windows.Forms.Label();
            this.txt_Recebedor = new System.Windows.Forms.TextBox();
            this.Btn_SalvarMerc = new System.Windows.Forms.Button();
            this.Btn_LimparMerc = new System.Windows.Forms.Button();
            this.Btn_AtualizarMerc = new System.Windows.Forms.Button();
            this.Btn_Entregue = new System.Windows.Forms.Button();
            this.Btn_FecharMerc = new System.Windows.Forms.Button();
            this.lbl_lista = new System.Windows.Forms.Label();
            this.lbl_legenda = new System.Windows.Forms.Label();
            this.dtg_mercadorias = new System.Windows.Forms.DataGridView();
            this.menu_mercadoria.SuspendLayout();
            this.grp_dados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_mercadorias)).BeginInit();
            this.SuspendLayout();
            //
            // menu_mercadoria
            //
            this.menu_mercadoria.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Strip_relatorio});
            this.menu_mercadoria.Location = new System.Drawing.Point(0, 0);
            this.menu_mercadoria.Name = "menu_mercadoria";
            this.menu_mercadoria.Size = new System.Drawing.Size(940, 24);
            this.menu_mercadoria.TabIndex = 10;
            this.menu_mercadoria.Text = "menu_mercadoria";
            //
            // Strip_relatorio
            //
            this.Strip_relatorio.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Relatorio_personalizado});
            this.Strip_relatorio.Name = "Strip_relatorio";
            this.Strip_relatorio.Size = new System.Drawing.Size(79, 20);
            this.Strip_relatorio.Text = "RELATORIO";
            //
            // Relatorio_personalizado
            //
            this.Relatorio_personalizado.Name = "Relatorio_personalizado";
            this.Relatorio_personalizado.Size = new System.Drawing.Size(192, 22);
            this.Relatorio_personalizado.Text = "RELATORIO PERSONALIZADO";
            this.Relatorio_personalizado.Click += new System.EventHandler(this.Relatorio_personalizado_Click);
            //
            // grp_dados
            //
            this.grp_dados.Controls.Add(this.lbl_destinatario);
            this.grp_dados.Controls.Add(this.txt_Destinatario);
            this.grp_dados.Controls.Add(this.lbl_empresa);
            this.grp_dados.Controls.Add(this.txt_Empresa);
            this.grp_dados.Controls.Add(this.lbl_entregador);
            this.grp_dados.Controls.Add(this.txt_Entregador);
            this.grp_dados.Controls.Add(this.lbl_recebedor);
            this.grp_dados.Controls.Add(this.txt_Recebedor);
            this.grp_dados.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_dados.Location = new System.Drawing.Point(12, 40);
            this.grp_dados.Name = "grp_dados";
            this.grp_dados.Size = new System.Drawing.Size(916, 110);
            this.grp_dados.TabIndex = 0;
            this.grp_dados.TabStop = false;
            this.grp_dados.Text = "CHEGADA DA MERCADORIA";
            //
            // lbl_destinatario
            //
            this.lbl_destinatario.AutoSize = true;
            this.lbl_destinatario.Location = new System.Drawing.Point(10, 32);
            this.lbl_destinatario.Name = "lbl_destinatario";
            this.lbl_destinatario.Size = new System.Drawing.Size(92, 20);
            this.lbl_destinatario.TabIndex = 100;
            this.lbl_destinatario.Text = "DESTINATÁRIO";
            //
            // txt_Destinatario
            //
            this.txt_Destinatario.Location = new System.Drawing.Point(185, 28);
            this.txt_Destinatario.Name = "txt_Destinatario";
            this.txt_Destinatario.Size = new System.Drawing.Size(280, 26);
            this.txt_Destinatario.TabIndex = 0;
            //
            // lbl_empresa
            //
            this.lbl_empresa.AutoSize = true;
            this.lbl_empresa.Location = new System.Drawing.Point(500, 32);
            this.lbl_empresa.Name = "lbl_empresa";
            this.lbl_empresa.Size = new System.Drawing.Size(112, 20);
            this.lbl_empresa.TabIndex = 101;
            this.lbl_empresa.Text = "NOME DA EMPRESA";
            //
            // txt_Empresa
            //
            this.txt_Empresa.Location = new System.Drawing.Point(650, 28);
            this.txt_Empresa.Name = "txt_Empresa";
            this.txt_Empresa.Size = new System.Drawing.Size(240, 26);
            this.txt_Empresa.TabIndex = 1;
            //
            // lbl_entregador
            //
            this.lbl_entregador.AutoSize = true;
            this.lbl_entregador.Location = new System.Drawing.Point(10, 70);
            this.lbl_entregador.Name = "lbl_entregador";
            this.lbl_entregador.Size = new System.Drawing.Size(140, 20);
            this.lbl_entregador.TabIndex = 102;
            this.lbl_entregador.Text = "NOME DO ENTREGADOR";
            //
            // txt_Entregador
            //
            this.txt_Entregador.Location = new System.Drawing.Point(185, 66);
            this.txt_Entregador.Name = "txt_Entregador";
            this.txt_Entregador.Size = new System.Drawing.Size(280, 26);
            this.txt_Entregador.TabIndex = 2;
            //
            // lbl_recebedor
            //
            this.lbl_recebedor.AutoSize = true;
            this.lbl_recebedor.Location = new System.Drawing.Point(500, 70);
            this.lbl_recebedor.Name = "lbl_recebedor";
            this.lbl_recebedor.Size = new System.Drawing.Size(95, 20);
            this.lbl_recebedor.TabIndex = 103;
            this.lbl_recebedor.Text = "RECEBIDO POR";
            //
            // txt_Recebedor
            //
            this.txt_Recebedor.BackColor = System.Drawing.SystemColors.Control;
            this.txt_Recebedor.Location = new System.Drawing.Point(650, 66);
            this.txt_Recebedor.Name = "txt_Recebedor";
            this.txt_Recebedor.ReadOnly = true;
            this.txt_Recebedor.Size = new System.Drawing.Size(240, 26);
            this.txt_Recebedor.TabIndex = 104;
            this.txt_Recebedor.TabStop = false;
            //
            // Btn_SalvarMerc
            //
            this.Btn_SalvarMerc.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_SalvarMerc.Location = new System.Drawing.Point(12, 162);
            this.Btn_SalvarMerc.Name = "Btn_SalvarMerc";
            this.Btn_SalvarMerc.Size = new System.Drawing.Size(140, 36);
            this.Btn_SalvarMerc.TabIndex = 3;
            this.Btn_SalvarMerc.Text = "SALVAR";
            this.Btn_SalvarMerc.UseVisualStyleBackColor = true;
            this.Btn_SalvarMerc.Click += new System.EventHandler(this.Btn_SalvarMerc_Click);
            //
            // Btn_LimparMerc
            //
            this.Btn_LimparMerc.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_LimparMerc.Location = new System.Drawing.Point(162, 162);
            this.Btn_LimparMerc.Name = "Btn_LimparMerc";
            this.Btn_LimparMerc.Size = new System.Drawing.Size(140, 36);
            this.Btn_LimparMerc.TabIndex = 4;
            this.Btn_LimparMerc.Text = "LIMPAR";
            this.Btn_LimparMerc.UseVisualStyleBackColor = true;
            this.Btn_LimparMerc.Click += new System.EventHandler(this.Btn_LimparMerc_Click);
            //
            // Btn_AtualizarMerc
            //
            this.Btn_AtualizarMerc.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_AtualizarMerc.Location = new System.Drawing.Point(312, 162);
            this.Btn_AtualizarMerc.Name = "Btn_AtualizarMerc";
            this.Btn_AtualizarMerc.Size = new System.Drawing.Size(140, 36);
            this.Btn_AtualizarMerc.TabIndex = 5;
            this.Btn_AtualizarMerc.Text = "ATUALIZAR";
            this.Btn_AtualizarMerc.UseVisualStyleBackColor = true;
            this.Btn_AtualizarMerc.Click += new System.EventHandler(this.Btn_AtualizarMerc_Click);
            //
            // Btn_Entregue
            //
            this.Btn_Entregue.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Entregue.Location = new System.Drawing.Point(462, 162);
            this.Btn_Entregue.Name = "Btn_Entregue";
            this.Btn_Entregue.Size = new System.Drawing.Size(240, 36);
            this.Btn_Entregue.TabIndex = 6;
            this.Btn_Entregue.Text = "CONFIRMAR ENTREGA";
            this.Btn_Entregue.UseVisualStyleBackColor = true;
            this.Btn_Entregue.Click += new System.EventHandler(this.Btn_Entregue_Click);
            //
            // Btn_FecharMerc
            //
            this.Btn_FecharMerc.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_FecharMerc.Location = new System.Drawing.Point(788, 162);
            this.Btn_FecharMerc.Name = "Btn_FecharMerc";
            this.Btn_FecharMerc.Size = new System.Drawing.Size(140, 36);
            this.Btn_FecharMerc.TabIndex = 7;
            this.Btn_FecharMerc.Text = "FECHAR";
            this.Btn_FecharMerc.UseVisualStyleBackColor = true;
            this.Btn_FecharMerc.Click += new System.EventHandler(this.Btn_FecharMerc_Click);
            //
            // lbl_lista
            //
            this.lbl_lista.AutoSize = true;
            this.lbl_lista.Font = new System.Drawing.Font("Arial Narrow", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_lista.Location = new System.Drawing.Point(12, 206);
            this.lbl_lista.Name = "lbl_lista";
            this.lbl_lista.Size = new System.Drawing.Size(196, 23);
            this.lbl_lista.TabIndex = 105;
            this.lbl_lista.Text = "MERCADORIAS DO DIA";
            //
            // lbl_legenda
            //
            this.lbl_legenda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_legenda.AutoSize = true;
            this.lbl_legenda.Font = new System.Drawing.Font("Arial Narrow", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_legenda.Location = new System.Drawing.Point(690, 211);
            this.lbl_legenda.Name = "lbl_legenda";
            this.lbl_legenda.Size = new System.Drawing.Size(238, 18);
            this.lbl_legenda.TabIndex = 106;
            this.lbl_legenda.Text = "LINHA EM VERDE = JÁ ENTREGUE";
            //
            // dtg_mercadorias
            //
            this.dtg_mercadorias.AllowUserToAddRows = false;
            this.dtg_mercadorias.AllowUserToDeleteRows = false;
            this.dtg_mercadorias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtg_mercadorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_mercadorias.Location = new System.Drawing.Point(12, 234);
            this.dtg_mercadorias.Name = "dtg_mercadorias";
            this.dtg_mercadorias.ReadOnly = true;
            this.dtg_mercadorias.Size = new System.Drawing.Size(916, 337);
            this.dtg_mercadorias.TabIndex = 8;
            this.dtg_mercadorias.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_mercadorias_CellDoubleClick);
            //
            // Frm_Mercadoria
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(940, 583);
            this.Controls.Add(this.grp_dados);
            this.Controls.Add(this.Btn_SalvarMerc);
            this.Controls.Add(this.Btn_LimparMerc);
            this.Controls.Add(this.Btn_AtualizarMerc);
            this.Controls.Add(this.Btn_Entregue);
            this.Controls.Add(this.Btn_FecharMerc);
            this.Controls.Add(this.lbl_lista);
            this.Controls.Add(this.lbl_legenda);
            this.Controls.Add(this.dtg_mercadorias);
            this.Controls.Add(this.menu_mercadoria);
            this.MainMenuStrip = this.menu_mercadoria;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Mercadoria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MERCADORIAS";
            this.Load += new System.EventHandler(this.Frm_Mercadoria_Load);
            this.menu_mercadoria.ResumeLayout(false);
            this.menu_mercadoria.PerformLayout();
            this.grp_dados.ResumeLayout(false);
            this.grp_dados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_mercadorias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menu_mercadoria;
        private System.Windows.Forms.ToolStripMenuItem Strip_relatorio;
        private System.Windows.Forms.ToolStripMenuItem Relatorio_personalizado;
        private System.Windows.Forms.GroupBox grp_dados;
        private System.Windows.Forms.Label lbl_destinatario;
        private System.Windows.Forms.TextBox txt_Destinatario;
        private System.Windows.Forms.Label lbl_empresa;
        private System.Windows.Forms.TextBox txt_Empresa;
        private System.Windows.Forms.Label lbl_entregador;
        private System.Windows.Forms.TextBox txt_Entregador;
        private System.Windows.Forms.Label lbl_recebedor;
        private System.Windows.Forms.TextBox txt_Recebedor;
        private System.Windows.Forms.Button Btn_SalvarMerc;
        private System.Windows.Forms.Button Btn_LimparMerc;
        private System.Windows.Forms.Button Btn_AtualizarMerc;
        private System.Windows.Forms.Button Btn_Entregue;
        private System.Windows.Forms.Button Btn_FecharMerc;
        private System.Windows.Forms.Label lbl_lista;
        private System.Windows.Forms.Label lbl_legenda;
        private System.Windows.Forms.DataGridView dtg_mercadorias;
    }
}
