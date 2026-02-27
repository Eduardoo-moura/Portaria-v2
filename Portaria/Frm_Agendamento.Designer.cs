namespace Portaria
{
    partial class Frm_Agendamento
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Agendamento));
            this.lbl_user = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lbl_NOME = new System.Windows.Forms.Label();
            this.txt_nome = new System.Windows.Forms.TextBox();
            this.txt_empresa = new System.Windows.Forms.Label();
            this.txt_emp = new System.Windows.Forms.TextBox();
            this.txt_data = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.agenda = new System.Windows.Forms.DataGridView();
            this.btn_atualizar = new System.Windows.Forms.Button();
            this.tempo = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.agenda)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_user
            // 
            this.lbl_user.AutoSize = true;
            this.lbl_user.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_user.Location = new System.Drawing.Point(13, 9);
            this.lbl_user.Name = "lbl_user";
            this.lbl_user.Size = new System.Drawing.Size(84, 29);
            this.lbl_user.TabIndex = 1;
            this.lbl_user.Text = "@user";
            this.lbl_user.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(405, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "AGENDAR";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbl_NOME
            // 
            this.lbl_NOME.AutoSize = true;
            this.lbl_NOME.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NOME.Location = new System.Drawing.Point(14, 67);
            this.lbl_NOME.Name = "lbl_NOME";
            this.lbl_NOME.Size = new System.Drawing.Size(56, 20);
            this.lbl_NOME.TabIndex = 6;
            this.lbl_NOME.Text = "NOME";
            this.lbl_NOME.Click += new System.EventHandler(this.lbl_NOME_Click);
            // 
            // txt_nome
            // 
            this.txt_nome.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_nome.Location = new System.Drawing.Point(108, 61);
            this.txt_nome.Name = "txt_nome";
            this.txt_nome.Size = new System.Drawing.Size(429, 26);
            this.txt_nome.TabIndex = 8;
            this.txt_nome.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txt_empresa
            // 
            this.txt_empresa.AutoSize = true;
            this.txt_empresa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_empresa.Location = new System.Drawing.Point(14, 99);
            this.txt_empresa.Name = "txt_empresa";
            this.txt_empresa.Size = new System.Drawing.Size(88, 20);
            this.txt_empresa.TabIndex = 9;
            this.txt_empresa.Text = "EMPRESA";
            // 
            // txt_emp
            // 
            this.txt_emp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_emp.Location = new System.Drawing.Point(108, 93);
            this.txt_emp.Name = "txt_emp";
            this.txt_emp.Size = new System.Drawing.Size(429, 26);
            this.txt_emp.TabIndex = 10;
            this.txt_emp.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txt_data
            // 
            this.txt_data.CustomFormat = "dd/MM/yyyy HH:mm";
            this.txt_data.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_data.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.txt_data.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txt_data.Location = new System.Drawing.Point(131, 131);
            this.txt_data.MinDate = new System.DateTime(2026, 1, 1, 0, 0, 0, 0);
            this.txt_data.Name = "txt_data";
            this.txt_data.ShowUpDown = true;
            this.txt_data.Size = new System.Drawing.Size(152, 26);
            this.txt_data.TabIndex = 12;
            this.txt_data.Value = new System.DateTime(2026, 1, 1, 0, 0, 0, 0);
            this.txt_data.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "DATA / HORA";
            // 
            // agenda
            // 
            this.agenda.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.agenda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.agenda.Location = new System.Drawing.Point(6, 225);
            this.agenda.Name = "agenda";
            this.agenda.Size = new System.Drawing.Size(531, 311);
            this.agenda.TabIndex = 14;
            this.agenda.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.agenda_CellContentClick);
            // 
            // btn_atualizar
            // 
            this.btn_atualizar.Location = new System.Drawing.Point(6, 192);
            this.btn_atualizar.Name = "btn_atualizar";
            this.btn_atualizar.Size = new System.Drawing.Size(85, 27);
            this.btn_atualizar.TabIndex = 15;
            this.btn_atualizar.Text = "ATUALIZAR";
            this.btn_atualizar.UseVisualStyleBackColor = true;
            this.btn_atualizar.Click += new System.EventHandler(this.button2_Click);
            // 
            // tempo
            // 
            this.tempo.Enabled = true;
            this.tempo.Interval = 5000;
            this.tempo.Tick += new System.EventHandler(this.tempo_Tick);
            // 
            // Frm_Agendamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 548);
            this.Controls.Add(this.btn_atualizar);
            this.Controls.Add(this.agenda);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_data);
            this.Controls.Add(this.txt_emp);
            this.Controls.Add(this.txt_empresa);
            this.Controls.Add(this.txt_nome);
            this.Controls.Add(this.lbl_NOME);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_user);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm_Agendamento";
            this.Text = "AGENDAMENTO";
            this.Load += new System.EventHandler(this.Frm_Agendamento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.agenda)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion
        private System.Windows.Forms.Label lbl_user;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbl_NOME;
        private System.Windows.Forms.TextBox txt_nome;
        private System.Windows.Forms.Label txt_empresa;
        private System.Windows.Forms.TextBox txt_emp;
        private System.Windows.Forms.DateTimePicker txt_data;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView agenda;
        private System.Windows.Forms.Button btn_atualizar;
        private System.Windows.Forms.Timer tempo;
    }
}