namespace Portaria
{
    partial class Frm_Usuarios
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
            this.grp_novo = new System.Windows.Forms.GroupBox();
            this.lbl_login = new System.Windows.Forms.Label();
            this.txt_login = new System.Windows.Forms.TextBox();
            this.lbl_nome = new System.Windows.Forms.Label();
            this.txt_nome = new System.Windows.Forms.TextBox();
            this.lbl_senha = new System.Windows.Forms.Label();
            this.txt_senha = new System.Windows.Forms.TextBox();
            this.lbl_confirma = new System.Windows.Forms.Label();
            this.txt_confirma = new System.Windows.Forms.TextBox();
            this.lbl_nivel = new System.Windows.Forms.Label();
            this.cmb_nivel = new System.Windows.Forms.ComboBox();
            this.btn_salvar = new System.Windows.Forms.Button();
            this.btn_limpar = new System.Windows.Forms.Button();
            this.lbl_lista = new System.Windows.Forms.Label();
            this.dtg_usuarios = new System.Windows.Forms.DataGridView();
            this.btn_senha = new System.Windows.Forms.Button();
            this.btn_nivel = new System.Windows.Forms.Button();
            this.btn_ativo = new System.Windows.Forms.Button();
            this.btn_fechar = new System.Windows.Forms.Button();
            this.grp_novo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_usuarios)).BeginInit();
            this.SuspendLayout();
            //
            // grp_novo
            //
            this.grp_novo.Controls.Add(this.lbl_login);
            this.grp_novo.Controls.Add(this.txt_login);
            this.grp_novo.Controls.Add(this.lbl_nome);
            this.grp_novo.Controls.Add(this.txt_nome);
            this.grp_novo.Controls.Add(this.lbl_senha);
            this.grp_novo.Controls.Add(this.txt_senha);
            this.grp_novo.Controls.Add(this.lbl_confirma);
            this.grp_novo.Controls.Add(this.txt_confirma);
            this.grp_novo.Controls.Add(this.lbl_nivel);
            this.grp_novo.Controls.Add(this.cmb_nivel);
            this.grp_novo.Controls.Add(this.btn_salvar);
            this.grp_novo.Controls.Add(this.btn_limpar);
            this.grp_novo.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_novo.Location = new System.Drawing.Point(12, 12);
            this.grp_novo.Name = "grp_novo";
            this.grp_novo.Size = new System.Drawing.Size(676, 195);
            this.grp_novo.TabIndex = 0;
            this.grp_novo.TabStop = false;
            this.grp_novo.Text = "NOVO USUARIO";
            //
            // lbl_login
            //
            this.lbl_login.AutoSize = true;
            this.lbl_login.Location = new System.Drawing.Point(12, 28);
            this.lbl_login.Name = "lbl_login";
            this.lbl_login.Size = new System.Drawing.Size(63, 20);
            this.lbl_login.TabIndex = 0;
            this.lbl_login.Text = "USUARIO";
            //
            // txt_login
            //
            this.txt_login.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_login.Location = new System.Drawing.Point(12, 51);
            this.txt_login.MaxLength = 40;
            this.txt_login.Name = "txt_login";
            this.txt_login.Size = new System.Drawing.Size(290, 26);
            this.txt_login.TabIndex = 1;
            //
            // lbl_nome
            //
            this.lbl_nome.AutoSize = true;
            this.lbl_nome.Location = new System.Drawing.Point(340, 28);
            this.lbl_nome.Name = "lbl_nome";
            this.lbl_nome.Size = new System.Drawing.Size(45, 20);
            this.lbl_nome.TabIndex = 2;
            this.lbl_nome.Text = "NOME";
            //
            // txt_nome
            //
            this.txt_nome.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_nome.Location = new System.Drawing.Point(340, 51);
            this.txt_nome.MaxLength = 60;
            this.txt_nome.Name = "txt_nome";
            this.txt_nome.Size = new System.Drawing.Size(320, 26);
            this.txt_nome.TabIndex = 3;
            //
            // lbl_senha
            //
            this.lbl_senha.AutoSize = true;
            this.lbl_senha.Location = new System.Drawing.Point(12, 88);
            this.lbl_senha.Name = "lbl_senha";
            this.lbl_senha.Size = new System.Drawing.Size(45, 20);
            this.lbl_senha.TabIndex = 4;
            this.lbl_senha.Text = "SENHA";
            //
            // txt_senha
            //
            this.txt_senha.Location = new System.Drawing.Point(12, 111);
            this.txt_senha.MaxLength = 60;
            this.txt_senha.Name = "txt_senha";
            this.txt_senha.Size = new System.Drawing.Size(290, 26);
            this.txt_senha.TabIndex = 5;
            this.txt_senha.UseSystemPasswordChar = true;
            //
            // lbl_confirma
            //
            this.lbl_confirma.AutoSize = true;
            this.lbl_confirma.Location = new System.Drawing.Point(340, 88);
            this.lbl_confirma.Name = "lbl_confirma";
            this.lbl_confirma.Size = new System.Drawing.Size(122, 20);
            this.lbl_confirma.TabIndex = 6;
            this.lbl_confirma.Text = "CONFIRMAR SENHA";
            //
            // txt_confirma
            //
            this.txt_confirma.Location = new System.Drawing.Point(340, 111);
            this.txt_confirma.MaxLength = 60;
            this.txt_confirma.Name = "txt_confirma";
            this.txt_confirma.Size = new System.Drawing.Size(320, 26);
            this.txt_confirma.TabIndex = 7;
            this.txt_confirma.UseSystemPasswordChar = true;
            //
            // lbl_nivel
            //
            this.lbl_nivel.AutoSize = true;
            this.lbl_nivel.Location = new System.Drawing.Point(12, 152);
            this.lbl_nivel.Name = "lbl_nivel";
            this.lbl_nivel.Size = new System.Drawing.Size(37, 20);
            this.lbl_nivel.TabIndex = 8;
            this.lbl_nivel.Text = "NIVEL";
            //
            // cmb_nivel
            //
            this.cmb_nivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_nivel.Location = new System.Drawing.Point(60, 149);
            this.cmb_nivel.Name = "cmb_nivel";
            this.cmb_nivel.Size = new System.Drawing.Size(242, 28);
            this.cmb_nivel.TabIndex = 9;
            //
            // btn_salvar
            //
            this.btn_salvar.Location = new System.Drawing.Point(400, 147);
            this.btn_salvar.Name = "btn_salvar";
            this.btn_salvar.Size = new System.Drawing.Size(130, 32);
            this.btn_salvar.TabIndex = 10;
            this.btn_salvar.Text = "SALVAR";
            this.btn_salvar.UseVisualStyleBackColor = true;
            this.btn_salvar.Click += new System.EventHandler(this.btn_salvar_Click);
            //
            // btn_limpar
            //
            this.btn_limpar.Location = new System.Drawing.Point(540, 147);
            this.btn_limpar.Name = "btn_limpar";
            this.btn_limpar.Size = new System.Drawing.Size(120, 32);
            this.btn_limpar.TabIndex = 11;
            this.btn_limpar.Text = "LIMPAR";
            this.btn_limpar.UseVisualStyleBackColor = true;
            this.btn_limpar.Click += new System.EventHandler(this.btn_limpar_Click);
            //
            // lbl_lista
            //
            this.lbl_lista.AutoSize = true;
            this.lbl_lista.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_lista.Location = new System.Drawing.Point(12, 217);
            this.lbl_lista.Name = "lbl_lista";
            this.lbl_lista.Size = new System.Drawing.Size(159, 20);
            this.lbl_lista.TabIndex = 1;
            this.lbl_lista.Text = "USUARIOS CADASTRADOS";
            //
            // dtg_usuarios
            //
            this.dtg_usuarios.AllowUserToAddRows = false;
            this.dtg_usuarios.AllowUserToDeleteRows = false;
            this.dtg_usuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtg_usuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtg_usuarios.Location = new System.Drawing.Point(12, 240);
            this.dtg_usuarios.MultiSelect = false;
            this.dtg_usuarios.Name = "dtg_usuarios";
            this.dtg_usuarios.ReadOnly = true;
            this.dtg_usuarios.RowHeadersVisible = false;
            this.dtg_usuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dtg_usuarios.Size = new System.Drawing.Size(676, 195);
            this.dtg_usuarios.TabIndex = 2;
            this.dtg_usuarios.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dtg_usuarios_CellDoubleClick);
            //
            // btn_senha
            //
            this.btn_senha.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_senha.Location = new System.Drawing.Point(12, 447);
            this.btn_senha.Name = "btn_senha";
            this.btn_senha.Size = new System.Drawing.Size(150, 32);
            this.btn_senha.TabIndex = 3;
            this.btn_senha.Text = "ALTERAR SENHA";
            this.btn_senha.UseVisualStyleBackColor = true;
            this.btn_senha.Click += new System.EventHandler(this.btn_senha_Click);
            //
            // btn_nivel
            //
            this.btn_nivel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_nivel.Location = new System.Drawing.Point(172, 447);
            this.btn_nivel.Name = "btn_nivel";
            this.btn_nivel.Size = new System.Drawing.Size(150, 32);
            this.btn_nivel.TabIndex = 4;
            this.btn_nivel.Text = "ALTERAR NIVEL";
            this.btn_nivel.UseVisualStyleBackColor = true;
            this.btn_nivel.Click += new System.EventHandler(this.btn_nivel_Click);
            //
            // btn_ativo
            //
            this.btn_ativo.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_ativo.Location = new System.Drawing.Point(332, 447);
            this.btn_ativo.Name = "btn_ativo";
            this.btn_ativo.Size = new System.Drawing.Size(180, 32);
            this.btn_ativo.TabIndex = 5;
            this.btn_ativo.Text = "ATIVAR / DESATIVAR";
            this.btn_ativo.UseVisualStyleBackColor = true;
            this.btn_ativo.Click += new System.EventHandler(this.btn_ativo_Click);
            //
            // btn_fechar
            //
            this.btn_fechar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_fechar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fechar.Location = new System.Drawing.Point(568, 447);
            this.btn_fechar.Name = "btn_fechar";
            this.btn_fechar.Size = new System.Drawing.Size(120, 32);
            this.btn_fechar.TabIndex = 6;
            this.btn_fechar.Text = "FECHAR";
            this.btn_fechar.UseVisualStyleBackColor = true;
            //
            // Frm_Usuarios
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CancelButton = this.btn_fechar;
            this.ClientSize = new System.Drawing.Size(700, 495);
            this.Controls.Add(this.btn_fechar);
            this.Controls.Add(this.btn_ativo);
            this.Controls.Add(this.btn_nivel);
            this.Controls.Add(this.btn_senha);
            this.Controls.Add(this.dtg_usuarios);
            this.Controls.Add(this.lbl_lista);
            this.Controls.Add(this.grp_novo);
            this.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_Usuarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CADASTRO DE USUARIOS";
            this.Load += new System.EventHandler(this.Frm_Usuarios_Load);
            this.grp_novo.ResumeLayout(false);
            this.grp_novo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtg_usuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_novo;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.TextBox txt_login;
        private System.Windows.Forms.Label lbl_nome;
        private System.Windows.Forms.TextBox txt_nome;
        private System.Windows.Forms.Label lbl_senha;
        private System.Windows.Forms.TextBox txt_senha;
        private System.Windows.Forms.Label lbl_confirma;
        private System.Windows.Forms.TextBox txt_confirma;
        private System.Windows.Forms.Label lbl_nivel;
        private System.Windows.Forms.ComboBox cmb_nivel;
        private System.Windows.Forms.Button btn_salvar;
        private System.Windows.Forms.Button btn_limpar;
        private System.Windows.Forms.Label lbl_lista;
        private System.Windows.Forms.DataGridView dtg_usuarios;
        private System.Windows.Forms.Button btn_senha;
        private System.Windows.Forms.Button btn_nivel;
        private System.Windows.Forms.Button btn_ativo;
        private System.Windows.Forms.Button btn_fechar;
    }
}
