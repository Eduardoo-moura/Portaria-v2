namespace Portaria
{
    partial class Frm_Veiculo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Veiculo));
            this.txt_NOME = new System.Windows.Forms.TextBox();
            this.txt_RG_A = new System.Windows.Forms.TextBox();
            this.txt_RG = new System.Windows.Forms.TextBox();
            this.TIPO = new System.Windows.Forms.TextBox();
            this.txt_Placa = new System.Windows.Forms.TextBox();
            this.txt_NOME_A = new System.Windows.Forms.TextBox();
            this.lbl_RG = new System.Windows.Forms.Label();
            this.lbl_NOME_MOTORISTA = new System.Windows.Forms.Label();
            this.lbl_RG2 = new System.Windows.Forms.Label();
            this.lbl_AJUDANTE = new System.Windows.Forms.Label();
            this.lbl_PLACA = new System.Windows.Forms.Label();
            this.lbl_TIPO_VEICULO = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_rg = new System.Windows.Forms.Button();
            this.txt_cel = new System.Windows.Forms.TextBox();
            this.lbl_cel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.AGREGADO = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.PRESTADOR = new System.Windows.Forms.ListBox();
            this.ultimas_visitas = new System.Windows.Forms.DataGridView();
            this.btn_visitas = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_OBS = new System.Windows.Forms.TextBox();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.dtg_agendamento = new System.Windows.Forms.DataGridView();
            this.Txt_agendamento = new System.Windows.Forms.Label();
            this.btn_atualizar = new System.Windows.Forms.Button();
            this.time_veiculo = new System.Windows.Forms.Timer(this.components);
            this.dt_historico = new System.Windows.Forms.DataGridView();
            this.lbl_historico = new System.Windows.Forms.Label();
            this.att_historico = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Strip_relatorio = new System.Windows.Forms.ToolStripMenuItem();
            this.Relatorio_data = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultimas_visitas)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_agendamento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_historico)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_NOME
            // 
            this.txt_NOME.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_NOME.Location = new System.Drawing.Point(78, 62);
            this.txt_NOME.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_NOME.Name = "txt_NOME";
            this.txt_NOME.Size = new System.Drawing.Size(560, 26);
            this.txt_NOME.TabIndex = 1;
            this.txt_NOME.TextChanged += new System.EventHandler(this.txt_NOME_TextChanged);
            // 
            // txt_RG_A
            // 
            this.txt_RG_A.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_RG_A.Location = new System.Drawing.Point(78, 22);
            this.txt_RG_A.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_RG_A.Name = "txt_RG_A";
            this.txt_RG_A.Size = new System.Drawing.Size(140, 26);
            this.txt_RG_A.TabIndex = 0;
            this.txt_RG_A.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txt_RG
            // 
            this.txt_RG.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_RG.Location = new System.Drawing.Point(78, 25);
            this.txt_RG.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_RG.Name = "txt_RG";
            this.txt_RG.Size = new System.Drawing.Size(184, 26);
            this.txt_RG.TabIndex = 0;
            this.txt_RG.TextChanged += new System.EventHandler(this.txt_RG_TextChanged);
            this.txt_RG.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_RG_KeyDown);
            // 
            // TIPO
            // 
            this.TIPO.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TIPO.Location = new System.Drawing.Point(110, 60);
            this.TIPO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TIPO.Name = "TIPO";
            this.TIPO.Size = new System.Drawing.Size(152, 26);
            this.TIPO.TabIndex = 1;
            this.TIPO.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // txt_Placa
            // 
            this.txt_Placa.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_Placa.Location = new System.Drawing.Point(110, 22);
            this.txt_Placa.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_Placa.Name = "txt_Placa";
            this.txt_Placa.Size = new System.Drawing.Size(152, 26);
            this.txt_Placa.TabIndex = 0;
            this.txt_Placa.TextChanged += new System.EventHandler(this.textBox5_TextChanged);
            // 
            // txt_NOME_A
            // 
            this.txt_NOME_A.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_NOME_A.Location = new System.Drawing.Point(78, 54);
            this.txt_NOME_A.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_NOME_A.Name = "txt_NOME_A";
            this.txt_NOME_A.Size = new System.Drawing.Size(560, 26);
            this.txt_NOME_A.TabIndex = 1;
            this.txt_NOME_A.TextChanged += new System.EventHandler(this.textBox6_TextChanged);
            // 
            // lbl_RG
            // 
            this.lbl_RG.AutoSize = true;
            this.lbl_RG.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RG.Location = new System.Drawing.Point(6, 31);
            this.lbl_RG.Name = "lbl_RG";
            this.lbl_RG.Size = new System.Drawing.Size(66, 20);
            this.lbl_RG.TabIndex = 6;
            this.lbl_RG.Text = "RG / CPF";
            this.lbl_RG.Click += new System.EventHandler(this.lbl_RG_Click);
            // 
            // lbl_NOME_MOTORISTA
            // 
            this.lbl_NOME_MOTORISTA.AutoSize = true;
            this.lbl_NOME_MOTORISTA.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NOME_MOTORISTA.Location = new System.Drawing.Point(6, 68);
            this.lbl_NOME_MOTORISTA.Name = "lbl_NOME_MOTORISTA";
            this.lbl_NOME_MOTORISTA.Size = new System.Drawing.Size(48, 20);
            this.lbl_NOME_MOTORISTA.TabIndex = 7;
            this.lbl_NOME_MOTORISTA.Text = "NOME";
            this.lbl_NOME_MOTORISTA.Click += new System.EventHandler(this.label2_Click);
            // 
            // lbl_RG2
            // 
            this.lbl_RG2.AutoSize = true;
            this.lbl_RG2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RG2.Location = new System.Drawing.Point(6, 28);
            this.lbl_RG2.Name = "lbl_RG2";
            this.lbl_RG2.Size = new System.Drawing.Size(66, 20);
            this.lbl_RG2.TabIndex = 8;
            this.lbl_RG2.Text = "RG / CPF";
            // 
            // lbl_AJUDANTE
            // 
            this.lbl_AJUDANTE.AutoSize = true;
            this.lbl_AJUDANTE.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_AJUDANTE.Location = new System.Drawing.Point(6, 60);
            this.lbl_AJUDANTE.Name = "lbl_AJUDANTE";
            this.lbl_AJUDANTE.Size = new System.Drawing.Size(48, 20);
            this.lbl_AJUDANTE.TabIndex = 9;
            this.lbl_AJUDANTE.Text = "NOME";
            // 
            // lbl_PLACA
            // 
            this.lbl_PLACA.AutoSize = true;
            this.lbl_PLACA.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PLACA.Location = new System.Drawing.Point(6, 26);
            this.lbl_PLACA.Name = "lbl_PLACA";
            this.lbl_PLACA.Size = new System.Drawing.Size(52, 20);
            this.lbl_PLACA.TabIndex = 0;
            this.lbl_PLACA.Text = "PLACA";
            // 
            // lbl_TIPO_VEICULO
            // 
            this.lbl_TIPO_VEICULO.AutoSize = true;
            this.lbl_TIPO_VEICULO.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TIPO_VEICULO.Location = new System.Drawing.Point(6, 66);
            this.lbl_TIPO_VEICULO.Name = "lbl_TIPO_VEICULO";
            this.lbl_TIPO_VEICULO.Size = new System.Drawing.Size(98, 20);
            this.lbl_TIPO_VEICULO.TabIndex = 11;
            this.lbl_TIPO_VEICULO.Text = "TIPO VEICULO";
            this.lbl_TIPO_VEICULO.Click += new System.EventHandler(this.label6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_rg);
            this.groupBox1.Controls.Add(this.txt_cel);
            this.groupBox1.Controls.Add(this.lbl_cel);
            this.groupBox1.Controls.Add(this.txt_RG);
            this.groupBox1.Controls.Add(this.txt_NOME);
            this.groupBox1.Controls.Add(this.lbl_RG);
            this.groupBox1.Controls.Add(this.lbl_NOME_MOTORISTA);
            this.groupBox1.Location = new System.Drawing.Point(12, 200);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(644, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MOTORISTA";
            // 
            // btn_rg
            // 
            this.btn_rg.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_rg.Location = new System.Drawing.Point(269, 20);
            this.btn_rg.Name = "btn_rg";
            this.btn_rg.Size = new System.Drawing.Size(47, 35);
            this.btn_rg.TabIndex = 10;
            this.btn_rg.Text = "🔎";
            this.btn_rg.UseVisualStyleBackColor = true;
            this.btn_rg.Click += new System.EventHandler(this.btn_rg_Click);
            // 
            // txt_cel
            // 
            this.txt_cel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_cel.Location = new System.Drawing.Point(470, 22);
            this.txt_cel.Name = "txt_cel";
            this.txt_cel.Size = new System.Drawing.Size(168, 26);
            this.txt_cel.TabIndex = 9;
            this.txt_cel.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // lbl_cel
            // 
            this.lbl_cel.AutoSize = true;
            this.lbl_cel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_cel.Location = new System.Drawing.Point(380, 28);
            this.lbl_cel.Name = "lbl_cel";
            this.lbl_cel.Size = new System.Drawing.Size(68, 20);
            this.lbl_cel.TabIndex = 8;
            this.lbl_cel.Text = "CELULAR";
            this.lbl_cel.Click += new System.EventHandler(this.label3_Click_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbl_AJUDANTE);
            this.groupBox2.Controls.Add(this.lbl_RG2);
            this.groupBox2.Controls.Add(this.txt_RG_A);
            this.groupBox2.Controls.Add(this.txt_NOME_A);
            this.groupBox2.Location = new System.Drawing.Point(12, 306);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(644, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "AJUDANTE";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.lbl_PLACA);
            this.groupBox3.Controls.Add(this.txt_Placa);
            this.groupBox3.Controls.Add(this.TIPO);
            this.groupBox3.Controls.Add(this.lbl_TIPO_VEICULO);
            this.groupBox3.Location = new System.Drawing.Point(12, 101);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(322, 100);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "VEICULO";
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(268, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 33);
            this.button1.TabIndex = 2;
            this.button1.Text = "🔎";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AGREGADO
            // 
            this.AGREGADO.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AGREGADO.FormattingEnabled = true;
            this.AGREGADO.ItemHeight = 20;
            this.AGREGADO.Items.AddRange(new object[] {
            "SIM",
            "NÃO"});
            this.AGREGADO.Location = new System.Drawing.Point(482, 150);
            this.AGREGADO.Name = "AGREGADO";
            this.AGREGADO.Size = new System.Drawing.Size(120, 44);
            this.AGREGADO.TabIndex = 7;
            this.AGREGADO.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(478, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "AGREGADO";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(352, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 40);
            this.label2.TabIndex = 4;
            this.label2.Text = "PRESTADOR\r\nDE SERVIÇO";
            this.label2.Click += new System.EventHandler(this.label2_Click_1);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(517, 416);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 33);
            this.button2.TabIndex = 9;
            this.button2.Text = "SALVAR";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btn_Salvar);
            // 
            // PRESTADOR
            // 
            this.PRESTADOR.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PRESTADOR.FormattingEnabled = true;
            this.PRESTADOR.ItemHeight = 20;
            this.PRESTADOR.Items.AddRange(new object[] {
            "SIM",
            "NAO"});
            this.PRESTADOR.Location = new System.Drawing.Point(340, 150);
            this.PRESTADOR.Name = "PRESTADOR";
            this.PRESTADOR.Size = new System.Drawing.Size(120, 44);
            this.PRESTADOR.TabIndex = 5;
            this.PRESTADOR.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // ultimas_visitas
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultimas_visitas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ultimas_visitas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ultimas_visitas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.ultimas_visitas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultimas_visitas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ultimas_visitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ultimas_visitas.Location = new System.Drawing.Point(12, 455);
            this.ultimas_visitas.Name = "ultimas_visitas";
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultimas_visitas.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.ultimas_visitas.Size = new System.Drawing.Size(1880, 529);
            this.ultimas_visitas.TabIndex = 23;
            this.ultimas_visitas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ultimas_visitas_CellContentClick);
            // 
            // btn_visitas
            // 
            this.btn_visitas.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_visitas.Location = new System.Drawing.Point(12, 416);
            this.btn_visitas.Name = "btn_visitas";
            this.btn_visitas.Size = new System.Drawing.Size(137, 33);
            this.btn_visitas.TabIndex = 24;
            this.btn_visitas.Text = "ATUALIZAR";
            this.btn_visitas.UseVisualStyleBackColor = true;
            this.btn_visitas.Click += new System.EventHandler(this.btn_visitas_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_OBS);
            this.groupBox4.Location = new System.Drawing.Point(660, 109);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(372, 74);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "EMPRESA";
            // 
            // txt_OBS
            // 
            this.txt_OBS.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_OBS.Location = new System.Drawing.Point(4, 43);
            this.txt_OBS.Name = "txt_OBS";
            this.txt_OBS.Size = new System.Drawing.Size(359, 26);
            this.txt_OBS.TabIndex = 0;
            this.txt_OBS.TextChanged += new System.EventHandler(this.textBox1_TextChanged_2);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // dtg_agendamento
            // 
            this.dtg_agendamento.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtg_agendamento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_agendamento.Location = new System.Drawing.Point(1044, 127);
            this.dtg_agendamento.Name = "dtg_agendamento";
            this.dtg_agendamento.Size = new System.Drawing.Size(848, 322);
            this.dtg_agendamento.TabIndex = 27;
            this.dtg_agendamento.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_agendamento_CellContentClick);
            // 
            // Txt_agendamento
            // 
            this.Txt_agendamento.AutoSize = true;
            this.Txt_agendamento.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txt_agendamento.Location = new System.Drawing.Point(1308, 88);
            this.Txt_agendamento.Name = "Txt_agendamento";
            this.Txt_agendamento.Size = new System.Drawing.Size(270, 31);
            this.Txt_agendamento.TabIndex = 28;
            this.Txt_agendamento.Text = "AGENDAMENTO DO DIA";
            this.Txt_agendamento.Click += new System.EventHandler(this.label3_Click);
            // 
            // btn_atualizar
            // 
            this.btn_atualizar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_atualizar.Location = new System.Drawing.Point(1584, 88);
            this.btn_atualizar.Name = "btn_atualizar";
            this.btn_atualizar.Size = new System.Drawing.Size(85, 27);
            this.btn_atualizar.TabIndex = 29;
            this.btn_atualizar.Text = "ATUALIZAR";
            this.btn_atualizar.UseVisualStyleBackColor = true;
            this.btn_atualizar.Click += new System.EventHandler(this.btn_atualizar_Click);
            // 
            // time_veiculo
            // 
            this.time_veiculo.Enabled = true;
            this.time_veiculo.Interval = 5000;
            this.time_veiculo.Tick += new System.EventHandler(this.time_veiculo_Tick);
            // 
            // dt_historico
            // 
            this.dt_historico.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dt_historico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dt_historico.Location = new System.Drawing.Point(660, 220);
            this.dt_historico.Name = "dt_historico";
            this.dt_historico.Size = new System.Drawing.Size(372, 229);
            this.dt_historico.TabIndex = 30;
            this.dt_historico.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dt_historico_CellContentClick);
            // 
            // lbl_historico
            // 
            this.lbl_historico.AutoSize = true;
            this.lbl_historico.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_historico.Location = new System.Drawing.Point(660, 194);
            this.lbl_historico.Name = "lbl_historico";
            this.lbl_historico.Size = new System.Drawing.Size(117, 20);
            this.lbl_historico.TabIndex = 31;
            this.lbl_historico.Text = "ULTIMAS VISITAS";
            // 
            // att_historico
            // 
            this.att_historico.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.att_historico.Location = new System.Drawing.Point(939, 189);
            this.att_historico.Name = "att_historico";
            this.att_historico.Size = new System.Drawing.Size(93, 30);
            this.att_historico.TabIndex = 32;
            this.att_historico.Text = "ATUALIZAR";
            this.att_historico.UseVisualStyleBackColor = true;
            this.att_historico.Click += new System.EventHandler(this.att_historico_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Strip_relatorio});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1904, 24);
            this.menuStrip1.TabIndex = 33;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Strip_relatorio
            // 
            this.Strip_relatorio.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Relatorio_data});
            this.Strip_relatorio.Name = "Strip_relatorio";
            this.Strip_relatorio.Size = new System.Drawing.Size(78, 20);
            this.Strip_relatorio.Text = "RELATORIO";
            this.Strip_relatorio.Click += new System.EventHandler(this.Strip_relatorio_Click);
            // 
            // Relatorio_data
            // 
            this.Relatorio_data.Name = "Relatorio_data";
            this.Relatorio_data.Size = new System.Drawing.Size(190, 22);
            this.Relatorio_data.Text = "RELATORIO POR DATA";
            this.Relatorio_data.Click += new System.EventHandler(this.Relatorio_data_Click);
            // 
            // Frm_Veiculo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1904, 990);
            this.Controls.Add(this.att_historico);
            this.Controls.Add(this.lbl_historico);
            this.Controls.Add(this.dt_historico);
            this.Controls.Add(this.btn_atualizar);
            this.Controls.Add(this.Txt_agendamento);
            this.Controls.Add(this.dtg_agendamento);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btn_visitas);
            this.Controls.Add(this.ultimas_visitas);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AGREGADO);
            this.Controls.Add(this.PRESTADOR);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_Veiculo";
            this.Text = "VEICULO";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Frm_Veiculo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultimas_visitas)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_agendamento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_historico)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_NOME;
        private System.Windows.Forms.TextBox txt_RG_A;
        private System.Windows.Forms.TextBox txt_RG;
        private System.Windows.Forms.TextBox TIPO;
        private System.Windows.Forms.TextBox txt_Placa;
        private System.Windows.Forms.TextBox txt_NOME_A;
        private System.Windows.Forms.Label lbl_RG;
        private System.Windows.Forms.Label lbl_NOME_MOTORISTA;
        private System.Windows.Forms.Label lbl_RG2;
        private System.Windows.Forms.Label lbl_AJUDANTE;
        private System.Windows.Forms.Label lbl_PLACA;
        private System.Windows.Forms.Label lbl_TIPO_VEICULO;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox AGREGADO;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox PRESTADOR;
        private System.Windows.Forms.DataGridView ultimas_visitas;
        private System.Windows.Forms.Button btn_visitas;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.DataGridView dtg_agendamento;
        private System.Windows.Forms.Label Txt_agendamento;
        private System.Windows.Forms.Button btn_atualizar;
        private System.Windows.Forms.Timer time_veiculo;
        private System.Windows.Forms.DataGridView dt_historico;
        private System.Windows.Forms.Label lbl_historico;
        private System.Windows.Forms.Button att_historico;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Strip_relatorio;
        private System.Windows.Forms.ToolStripMenuItem Relatorio_data;
        private System.Windows.Forms.Label lbl_cel;
        private System.Windows.Forms.TextBox txt_cel;
        private System.Windows.Forms.Button btn_rg;
        private System.Windows.Forms.TextBox txt_OBS;
    }
}