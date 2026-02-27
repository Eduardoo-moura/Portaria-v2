namespace Portaria
{
    partial class Frm_relatorio_data
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_relatorio_data));
            this.Data_inicio = new System.Windows.Forms.DateTimePicker();
            this.btn_gerar = new System.Windows.Forms.Button();
            this.lbl_data_inicio = new System.Windows.Forms.Label();
            this.data_final = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Data_inicio
            // 
            this.Data_inicio.CustomFormat = "";
            this.Data_inicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Data_inicio.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Data_inicio.Location = new System.Drawing.Point(136, 24);
            this.Data_inicio.Name = "Data_inicio";
            this.Data_inicio.Size = new System.Drawing.Size(124, 26);
            this.Data_inicio.TabIndex = 0;
            this.Data_inicio.Value = new System.DateTime(2026, 1, 1, 14, 2, 0, 0);
            this.Data_inicio.ValueChanged += new System.EventHandler(this.Data_inicio_ValueChanged);
            // 
            // btn_gerar
            // 
            this.btn_gerar.Location = new System.Drawing.Point(456, 25);
            this.btn_gerar.Name = "btn_gerar";
            this.btn_gerar.Size = new System.Drawing.Size(102, 29);
            this.btn_gerar.TabIndex = 2;
            this.btn_gerar.Text = "GERAR";
            this.btn_gerar.UseVisualStyleBackColor = true;
            this.btn_gerar.Click += new System.EventHandler(this.btn_gerar_Click);
            // 
            // lbl_data_inicio
            // 
            this.lbl_data_inicio.AutoSize = true;
            this.lbl_data_inicio.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_data_inicio.Location = new System.Drawing.Point(12, 31);
            this.lbl_data_inicio.Name = "lbl_data_inicio";
            this.lbl_data_inicio.Size = new System.Drawing.Size(83, 20);
            this.lbl_data_inicio.TabIndex = 14;
            this.lbl_data_inicio.Text = "PERIODO";
            this.lbl_data_inicio.Click += new System.EventHandler(this.lbl_data_inicio_Click);
            // 
            // data_final
            // 
            this.data_final.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.data_final.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.data_final.Location = new System.Drawing.Point(306, 25);
            this.data_final.Name = "data_final";
            this.data_final.Size = new System.Drawing.Size(117, 26);
            this.data_final.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(266, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "À";
            // 
            // Frm_relatorio_data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 79);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.data_final);
            this.Controls.Add(this.lbl_data_inicio);
            this.Controls.Add(this.btn_gerar);
            this.Controls.Add(this.Data_inicio);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Frm_relatorio_data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "RELATORIO POR DATA";
            this.Load += new System.EventHandler(this.Frm_relatorio_data_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker Data_inicio;
        private System.Windows.Forms.Button btn_gerar;
        private System.Windows.Forms.Label lbl_data_inicio;
        private System.Windows.Forms.DateTimePicker data_final;
        private System.Windows.Forms.Label label1;
    }
}