namespace Portaria
{
    partial class Frm_relatorio_mercadoria
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lbl_periodo = new System.Windows.Forms.Label();
            this.lbl_de = new System.Windows.Forms.Label();
            this.Data_inicio = new System.Windows.Forms.DateTimePicker();
            this.lbl_ate = new System.Windows.Forms.Label();
            this.data_final = new System.Windows.Forms.DateTimePicker();
            this.lbl_usuario = new System.Windows.Forms.Label();
            this.cmb_usuario = new System.Windows.Forms.ComboBox();
            this.grp_campos = new System.Windows.Forms.GroupBox();
            this.clb_campos = new System.Windows.Forms.CheckedListBox();
            this.btn_marcar = new System.Windows.Forms.Button();
            this.btn_desmarcar = new System.Windows.Forms.Button();
            this.chk_sem_entrega = new System.Windows.Forms.CheckBox();
            this.btn_gerar = new System.Windows.Forms.Button();
            this.btn_fechar = new System.Windows.Forms.Button();
            this.lbl_dica = new System.Windows.Forms.Label();
            this.grp_campos.SuspendLayout();
            this.SuspendLayout();
            //
            // lbl_periodo
            //
            this.lbl_periodo.AutoSize = true;
            this.lbl_periodo.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_periodo.Location = new System.Drawing.Point(12, 12);
            this.lbl_periodo.Name = "lbl_periodo";
            this.lbl_periodo.Size = new System.Drawing.Size(66, 22);
            this.lbl_periodo.TabIndex = 0;
            this.lbl_periodo.Text = "PERIODO";
            //
            // lbl_de
            //
            this.lbl_de.AutoSize = true;
            this.lbl_de.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_de.Location = new System.Drawing.Point(12, 47);
            this.lbl_de.Name = "lbl_de";
            this.lbl_de.Size = new System.Drawing.Size(23, 20);
            this.lbl_de.TabIndex = 1;
            this.lbl_de.Text = "DE";
            //
            // Data_inicio
            //
            this.Data_inicio.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Data_inicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.Data_inicio.Location = new System.Drawing.Point(45, 44);
            this.Data_inicio.Name = "Data_inicio";
            this.Data_inicio.Size = new System.Drawing.Size(160, 26);
            this.Data_inicio.TabIndex = 2;
            //
            // lbl_ate
            //
            this.lbl_ate.AutoSize = true;
            this.lbl_ate.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ate.Location = new System.Drawing.Point(220, 47);
            this.lbl_ate.Name = "lbl_ate";
            this.lbl_ate.Size = new System.Drawing.Size(29, 20);
            this.lbl_ate.TabIndex = 3;
            this.lbl_ate.Text = "ATE";
            //
            // data_final
            //
            this.data_final.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.data_final.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.data_final.Location = new System.Drawing.Point(255, 44);
            this.data_final.Name = "data_final";
            this.data_final.Size = new System.Drawing.Size(160, 26);
            this.data_final.TabIndex = 4;
            //
            // lbl_usuario
            //
            this.lbl_usuario.AutoSize = true;
            this.lbl_usuario.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_usuario.Location = new System.Drawing.Point(12, 84);
            this.lbl_usuario.Name = "lbl_usuario";
            this.lbl_usuario.Size = new System.Drawing.Size(158, 20);
            this.lbl_usuario.TabIndex = 5;
            this.lbl_usuario.Text = "USUARIO DA CHEGADA";
            //
            // cmb_usuario
            //
            this.cmb_usuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_usuario.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_usuario.FormattingEnabled = true;
            this.cmb_usuario.Location = new System.Drawing.Point(176, 81);
            this.cmb_usuario.Name = "cmb_usuario";
            this.cmb_usuario.Size = new System.Drawing.Size(239, 28);
            this.cmb_usuario.TabIndex = 6;
            //
            // grp_campos
            //
            this.grp_campos.Controls.Add(this.clb_campos);
            this.grp_campos.Controls.Add(this.btn_marcar);
            this.grp_campos.Controls.Add(this.btn_desmarcar);
            this.grp_campos.Controls.Add(this.chk_sem_entrega);
            this.grp_campos.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_campos.Location = new System.Drawing.Point(12, 119);
            this.grp_campos.Name = "grp_campos";
            this.grp_campos.Size = new System.Drawing.Size(596, 375);
            this.grp_campos.TabIndex = 7;
            this.grp_campos.TabStop = false;
            this.grp_campos.Text = "CAMPOS QUE VAO SAIR NO RELATORIO";
            //
            // clb_campos
            //
            this.clb_campos.CheckOnClick = true;
            this.clb_campos.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clb_campos.Location = new System.Drawing.Point(15, 28);
            this.clb_campos.Name = "clb_campos";
            this.clb_campos.Size = new System.Drawing.Size(566, 256);
            this.clb_campos.TabIndex = 0;
            //
            // btn_marcar
            //
            this.btn_marcar.Location = new System.Drawing.Point(15, 294);
            this.btn_marcar.Name = "btn_marcar";
            this.btn_marcar.Size = new System.Drawing.Size(150, 32);
            this.btn_marcar.TabIndex = 1;
            this.btn_marcar.Text = "MARCAR TODOS";
            this.btn_marcar.UseVisualStyleBackColor = true;
            this.btn_marcar.Click += new System.EventHandler(this.btn_marcar_Click);
            //
            // btn_desmarcar
            //
            this.btn_desmarcar.Location = new System.Drawing.Point(175, 294);
            this.btn_desmarcar.Name = "btn_desmarcar";
            this.btn_desmarcar.Size = new System.Drawing.Size(170, 32);
            this.btn_desmarcar.TabIndex = 2;
            this.btn_desmarcar.Text = "DESMARCAR TODOS";
            this.btn_desmarcar.UseVisualStyleBackColor = true;
            this.btn_desmarcar.Click += new System.EventHandler(this.btn_desmarcar_Click);
            //
            // chk_sem_entrega
            //
            this.chk_sem_entrega.AutoSize = true;
            this.chk_sem_entrega.Location = new System.Drawing.Point(15, 338);
            this.chk_sem_entrega.Name = "chk_sem_entrega";
            this.chk_sem_entrega.Size = new System.Drawing.Size(310, 24);
            this.chk_sem_entrega.TabIndex = 3;
            this.chk_sem_entrega.Text = "SOMENTE AS QUE AINDA ESTAO NA PORTARIA";
            this.chk_sem_entrega.UseVisualStyleBackColor = true;
            //
            // btn_gerar
            //
            this.btn_gerar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_gerar.Location = new System.Drawing.Point(12, 507);
            this.btn_gerar.Name = "btn_gerar";
            this.btn_gerar.Size = new System.Drawing.Size(200, 40);
            this.btn_gerar.TabIndex = 8;
            this.btn_gerar.Text = "GERAR RELATORIO";
            this.btn_gerar.UseVisualStyleBackColor = true;
            this.btn_gerar.Click += new System.EventHandler(this.btn_gerar_Click);
            //
            // btn_fechar
            //
            this.btn_fechar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_fechar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fechar.Location = new System.Drawing.Point(222, 507);
            this.btn_fechar.Name = "btn_fechar";
            this.btn_fechar.Size = new System.Drawing.Size(140, 40);
            this.btn_fechar.TabIndex = 9;
            this.btn_fechar.Text = "FECHAR";
            this.btn_fechar.UseVisualStyleBackColor = true;
            //
            // lbl_dica
            //
            this.lbl_dica.AutoSize = true;
            this.lbl_dica.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_dica.Location = new System.Drawing.Point(12, 557);
            this.lbl_dica.Name = "lbl_dica";
            this.lbl_dica.Size = new System.Drawing.Size(400, 16);
            this.lbl_dica.TabIndex = 10;
            this.lbl_dica.Text = "O período usa a data de chegada da mercadoria na portaria.";
            //
            // Frm_relatorio_mercadoria
            //
            this.AcceptButton = this.btn_gerar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CancelButton = this.btn_fechar;
            this.ClientSize = new System.Drawing.Size(620, 585);
            this.Controls.Add(this.lbl_dica);
            this.Controls.Add(this.btn_fechar);
            this.Controls.Add(this.btn_gerar);
            this.Controls.Add(this.grp_campos);
            this.Controls.Add(this.cmb_usuario);
            this.Controls.Add(this.lbl_usuario);
            this.Controls.Add(this.data_final);
            this.Controls.Add(this.lbl_ate);
            this.Controls.Add(this.Data_inicio);
            this.Controls.Add(this.lbl_de);
            this.Controls.Add(this.lbl_periodo);
            this.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_relatorio_mercadoria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RELATORIO DE MERCADORIAS";
            this.Load += new System.EventHandler(this.Frm_relatorio_mercadoria_Load);
            this.grp_campos.ResumeLayout(false);
            this.grp_campos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_periodo;
        private System.Windows.Forms.Label lbl_de;
        private System.Windows.Forms.DateTimePicker Data_inicio;
        private System.Windows.Forms.Label lbl_ate;
        private System.Windows.Forms.DateTimePicker data_final;
        private System.Windows.Forms.Label lbl_usuario;
        private System.Windows.Forms.ComboBox cmb_usuario;
        private System.Windows.Forms.GroupBox grp_campos;
        private System.Windows.Forms.CheckedListBox clb_campos;
        private System.Windows.Forms.Button btn_marcar;
        private System.Windows.Forms.Button btn_desmarcar;
        private System.Windows.Forms.CheckBox chk_sem_entrega;
        private System.Windows.Forms.Button btn_gerar;
        private System.Windows.Forms.Button btn_fechar;
        private System.Windows.Forms.Label lbl_dica;
    }
}
