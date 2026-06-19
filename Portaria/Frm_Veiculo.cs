using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Vml.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using static Portaria.Program;






namespace Portaria
{
    public partial class Frm_Veiculo : Form
    {
        public Frm_Veiculo()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.MinimizeBox = true; // opcional

            string conexao = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "controleAcesso.db")};Version=3;";
            string conexaoagenda = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "controleAcesso.db")};Version=3;";


            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.CharacterCasing = CharacterCasing.Upper;
                }
            }
                       
            
        }

        private readonly string conexao =
        @"Data Source=ControleAcesso.db;";

        private readonly string conexaoagenda =
        @"Data Source=ControleAcesso.db;";

        public void LimparCampo()
        {
            txt_Placa.Clear();
            txt_RG.Clear();
            txt_NOME.Clear();
            txt_RG_A.Clear();
            txt_NOME_A.Clear();
            txt_OBS.Clear();
            TIPO.Clear();
            txt_cel.Clear();

            foreach (TabPage tab in Tab_Ajudantes.TabPages)
            {
                foreach (Control ctrl in tab.Controls)
                {
                    if (ctrl is TextBox txt)
                        txt.Clear();
                }
            }
        }
        
        
        private void txt_RG_TextChanged(object sender, EventArgs e)
        {
            string txt_RG = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
            AcceptButton = btn_rg;

        }
        private void txt_NOME_TextChanged(object sender, EventArgs e)
        {
            string NOME_MOTORISTA = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string RG2 = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string NOME_AJUDANTE = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string TIPO_VEICULO = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string AGREGADO = Console.ReadLine();
            TextBox txt = sender as TextBox;

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click_1(object sender, EventArgs e)
        {

        }
        private void txt_RG_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string Placa = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
            this.KeyPreview = true;
            AcceptButton = button1;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PRESTADOR = Console.ReadLine();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string EMPRESA = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
            this.KeyPreview = true;
        }
        private void btn_Salvar(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_NOME.Text))
            {
                MessageBox.Show("O campo NOME é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_NOME.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_RG.Text))
            {
                MessageBox.Show("O campo RG ou CPF é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_RG.Focus();
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                string sql = @"
                INSERT INTO Veiculo
                (CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA)
                VALUES
                (@CPF, @NOME, @CELULAR, @CPFAJUDANTE, @NOMEAJUDANTE, @DataHora, @SAIDA, @PLACA, @TIPOVEICULO, @PRESTADOR, @AGREGADO, @EMPRESA)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CPF", txt_RG.Text);
                    cmd.Parameters.AddWithValue("@NOME", txt_NOME.Text);
                    cmd.Parameters.AddWithValue("@CELULAR", txt_cel.Text);
                    cmd.Parameters.AddWithValue("@CPFAJUDANTE", txt_RG_A.Text);
                    cmd.Parameters.AddWithValue("@NOMEAJUDANTE", txt_NOME_A.Text);
                    cmd.Parameters.AddWithValue("@DataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@SAIDA", "");
                    cmd.Parameters.AddWithValue("@PLACA", txt_Placa.Text);
                    cmd.Parameters.AddWithValue("@TIPOVEICULO", TIPO.Text);
                    cmd.Parameters.AddWithValue("@PRESTADOR", PRESTADOR.Text);
                    cmd.Parameters.AddWithValue("@AGREGADO", AGREGADO.Text);
                    cmd.Parameters.AddWithValue("@EMPRESA", txt_OBS.Text.Trim());
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("CADASTRADO!");
                LimparCampo();

                btn_visitas.PerformClick(); // ← recarrega o DataGrid

            }

        }
        private void btn_visitas_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();
                    string sql = @"
                    SELECT ID, CPF, NOME, CELULAR, 
                    CPFAJUDANTE AS 'CPF AJUDANTE', 
                    NOMEAJUDANTE AS 'NOME AJUDANTE', 
                    strftime('%d/%m/%Y %H:%M', DataHora) AS 'ENTRADA', 
                    SAIDA, PLACA, TIPOVEICULO AS 'TIPO VEICULO', 
                    PRESTADOR, AGREGADO, EMPRESA
                    FROM Veiculo
                    WHERE DATE(DataHora) = DATE('now')
                    ORDER BY DataHora DESC";

                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ultimas_visitas.DataSource = dt;

                        // Oculta ID somente se a coluna existir
                        if (ultimas_visitas.Columns["ID"] != null)
                            ultimas_visitas.Columns["ID"].Visible = false;

                        ultimas_visitas.ReadOnly = true;
                        ultimas_visitas.DefaultCellStyle.Font = new Font("Segoe UI", 12);
                        ultimas_visitas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                        ultimas_visitas.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                        ultimas_visitas.AlternatingRowsDefaultCellStyle = ultimas_visitas.DefaultCellStyle;
                        ultimas_visitas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                    }

                    if (OcultarVisitas.Checked = false)
                    {

                        btn_atualizar.PerformClick();
                        OcultarVisitas.Checked = false;
                    }
                    else
                    {
                        btn_atualizar.PerformClick();
                        OcultarVisitas.Checked = true;
                    }
                                        
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }

            

        }
        private void ultimas_visitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ultimas_visitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string placaProcurada = txt_Placa.Text.Trim().ToUpper();

            using (var con = new SQLiteConnection(conexao))
            {
                con.Open();
                string sql = @"
                    SELECT ID, CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, EMPRESA
                    FROM Veiculo
                    WHERE PLACA = @PLACA
                    ORDER BY DataHora DESC";

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@PLACA", placaProcurada);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txt_RG.Text = dr["CPF"].ToString();
                            txt_NOME.Text = dr["NOME"].ToString();
                            txt_cel.Text = dr["CELULAR"].ToString();
                            txt_RG_A.Text = dr["CPFAJUDANTE"].ToString();
                            txt_NOME_A.Text = dr["NOMEAJUDANTE"].ToString();
                            TIPO.Text = dr["TIPOVEICULO"].ToString();
                            txt_OBS.Text = dr["EMPRESA"].ToString().Trim();

                            MessageBox.Show("Registro encontrado!");
                            att_historico.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Placa não encontrada!");
                            LimparCampo();
                        }
                    }
                }
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void dtg_agendamento_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btn_atualizar_Click(object sender, EventArgs e)
        {
            string conexaoagenda = @"Data Source=ControleAcesso.db";

            DateTime hojeInicio = DateTime.Today;
            DateTime hojeFim = DateTime.Today.AddDays(1).AddSeconds(-1);

            string sql = @"
            SELECT USUARIO, NOME, EMPRESA,
            strftime('%d/%m/%Y %H:%M', DATAHORA) AS DATAHORA
            FROM AGENDAMENTO
            WHERE datetime(DATAHORA) BETWEEN datetime($inicio) AND datetime($fim)
            ORDER BY DATAHORA DESC";

            using (var conn = new SQLiteConnection(conexaoagenda))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("$usuario", Environment.UserName);
                    cmd.Parameters.AddWithValue("$inicio", hojeInicio.ToString("yyyy-MM-dd HH:mm"));
                    cmd.Parameters.AddWithValue("$fim", hojeFim.ToString("yyyy-MM-dd HH:mm"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        dtg_agendamento.DataSource = dt;
                    }
                }
            }

            // 4️⃣ CONFIGURAÇÕES VISUAIS
            dtg_agendamento.Columns[0].Visible = true;
            dtg_agendamento.ReadOnly = true;
            dtg_agendamento.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dtg_agendamento.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dtg_agendamento.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dtg_agendamento.AlternatingRowsDefaultCellStyle = dtg_agendamento.DefaultCellStyle;
        }
        private void time_veiculo_Tick(object sender, EventArgs e)
        {
            btn_visitas.PerformClick(); // auto-clique
            btn_atualizar.PerformClick(); // auto-clique

        }

        private void dt_historico_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Exemplo 4: Corrija o método att_historico_Click:
        private void att_historico_Click(object sender, EventArgs e)
        {
            string placaFiltro = txt_Placa.Text.Trim().ToUpper();
            string rgFiltro = txt_RG.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(placaFiltro) && string.IsNullOrEmpty(rgFiltro))
            {
                MessageBox.Show("Informe placa ou documento para pesquisa.");
                return;
            }

            try
            {
                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();

                    string sql;
                    using (var cmd = new SQLiteCommand(con))
                    {
                        if (!string.IsNullOrEmpty(placaFiltro))
                        {
                            sql = @"
                                SELECT 
                                strftime('%d/%m/%Y %H:%M', DataHora) AS 'ENTRADA', SAIDA
                                FROM Veiculo
                                WHERE UPPER(Placa) = $placa
                                ORDER BY DataHora DESC";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("$placa", placaFiltro);
                        }
                        else
                        {
                            sql = @"
                                SELECT 
                                strftime('%d/%m/%Y %H:%M', DataHora) AS 'ENTRADA', SAIDA
                                FROM Veiculo
                                WHERE UPPER(CPF) = $CPF
                                ORDER BY DataHora DESC";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("$CPF", rgFiltro);
                        }

                        DataTable dt = new DataTable();
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt_historico.DataSource = null;
                            dt_historico.Columns.Clear();

                            MessageBox.Show("Nenhum registro encontrado.");
                        }
                        else
                        {
                            dt_historico.DataSource = dt;
                            dt_historico.ReadOnly = true;
                            dt_historico.DefaultCellStyle.Font = new Font("Segoe UI", 12);
                            dt_historico.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                            dt_historico.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }

            dt_historico.ReadOnly = true;
            dt_historico.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dt_historico.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dt_historico.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dt_historico.AlternatingRowsDefaultCellStyle = dt_historico.DefaultCellStyle;

        }

        private void Frm_Veiculo_Load(object sender, EventArgs e)
        {

        }

        private void Relatorio_data_Click(object sender, EventArgs e)
        {
            Frm_relatorio_data f = new Frm_relatorio_data();
            f.ShowDialog();
        }

        private void Strip_relatorio_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void lbl_RG_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            string txt_cel = Console.ReadLine(); // Alterado de int para string
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
            this.KeyPreview = true;
        }

        // Exemplo 5: Corrija o método btn_rg_Click:
        private void btn_rg_Click(object sender, EventArgs e)
        {
            string placaCPF = txt_RG.Text.Trim().ToUpper();

            using (var con = new SQLiteConnection(conexao))
            {
                con.Open();
                string sql = @"
                    SELECT ID ,CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, EMPRESA
                    FROM Veiculo
                    WHERE @CPF = CPF
                    ORDER BY DataHora DESC";

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CPF", placaCPF);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txt_Placa.Text = dr["PLACA"].ToString();
                            txt_RG.Text = dr["CPF"].ToString();
                            txt_NOME.Text = dr["NOME"].ToString();
                            txt_cel.Text = dr["CELULAR"].ToString();
                            txt_RG_A.Text = dr["CPFAJUDANTE"].ToString();
                            txt_NOME_A.Text = dr["NOMEAJUDANTE"].ToString();
                            TIPO.Text = dr["TIPOVEICULO"].ToString();
                            txt_OBS.Text = dr["EMPRESA"].ToString().Trim();

                            MessageBox.Show("Registro encontrado!");
                            att_historico.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Documento não encontrado!");
                            LimparCampo();
                        }
                    }
                }
            }
        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

            string EMPRESA = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição
            this.KeyPreview = true;
        }

        private void lbl_RG2_Click(object sender, EventArgs e)
        {

        }

        private void Tab_Ajudante1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private int contador = 2;

        private void Btn_AbaAjudante_Click(object sender, EventArgs e)
        {
            List<int> numeros = new List<int>();

            foreach (TabPage tab in Tab_Ajudantes.TabPages)
            {
                string nome = tab.Text.Replace("AJUDANTE ", "");
                if (int.TryParse(nome, out int n))
                    numeros.Add(n);
            }

            int i = 1;
            while (numeros.Contains(i))
                i++;

            TabPage novaAba = new TabPage($"AJUDANTE {i}");

            // === LABEL RG/CPF ===
            Label lblRg = new Label();
            lblRg.Text = "RG / CPF";
            lblRg.Location = new Point(5, 10);
            lblRg.AutoSize = true;

            // === TEXTBOX RG/CPF (mesmo tamanho do Ajudante 1) ===
            TextBox txtRg = new TextBox();
            txtRg.Name = $"txtRgAjudante{i}";
            txtRg.Location = new Point(70, 5);
            txtRg.Size = new Size(200, 50);  // ← ajuste conforme o real
            txtRg.CharacterCasing = CharacterCasing.Upper;

            // === LABEL NOME ===
            Label lblNome = new Label();
            lblNome.Text = "NOME";
            lblNome.Location = new Point(10, 40);
            lblNome.AutoSize = true;

            // === TEXTBOX NOME (mesmo tamanho do Ajudante 1) ===
            TextBox txtNome = new TextBox();
            txtNome.Name = $"txtNomeAjudante{i}";
            txtNome.Location = new Point(70, 40);
            txtNome.Size = new Size(600, 50);  // ← ajuste conforme o real
            txtNome.CharacterCasing = CharacterCasing.Upper;

            novaAba.Controls.Add(lblRg);
            novaAba.Controls.Add(txtRg);
            novaAba.Controls.Add(lblNome);
            novaAba.Controls.Add(txtNome);

            Tab_Ajudantes.TabPages.Add(novaAba);
            Tab_Ajudantes.SelectedTab = novaAba;

            contador = i + 1;

        }

        private void FecharAjudante_Click(object sender, EventArgs e)
        {
            // Corrija o acesso: Tab_Ajudante1 é um TabPage, não um TabControl.
            // Para remover uma aba, você deve acessar o TabControl (por exemplo, Tab_Ajudantes).
            // Verifique se há mais de uma aba antes de remover.
            if (Tab_Ajudantes.TabPages.Count > 1)
            {
                Tab_Ajudantes.TabPages.RemoveAt(Tab_Ajudantes.TabPages.Count - 1);
            }
        }

        private void Btn_Saida_Click(object sender, EventArgs e)
        {
            if (ultimas_visitas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione uma linha!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var id = ultimas_visitas.SelectedRows[0].Cells["ID"].Value;

            if (id == null)
            {
                MessageBox.Show("Registro válido!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirma = MessageBox.Show("Confirmar saída?", "Confirmação",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirma != DialogResult.Yes) return;

            try
            {
                string dataHoraAtual = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();
                    string sql = "UPDATE Veiculo SET SAIDA = @saida WHERE ID = @id";

                    using (var cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@saida", dataHoraAtual);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Saída registrada!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                btn_visitas.PerformClick(); // ← recarrega o DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ultimas_visitas_CellContentClick()
        {
            throw new NotImplementedException();
        }

        private void Btn_CriarID_Click_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();

                    // 1. Cria tabela nova com ID
                    var sql1 = new SQLiteCommand(@"
                    CREATE TABLE IF NOT EXISTS Veiculo_Nova (
                    ID           INTEGER PRIMARY KEY AUTOINCREMENT,
                    CPF          TEXT,
                    NOME         TEXT,
                    CELULAR      TEXT,
                    CPFAJUDANTE  TEXT,
                    NOMEAJUDANTE TEXT,
                    DataHora     TEXT,
                    SAIDA        TEXT,
                    PLACA        TEXT,
                    TIPOVEICULO  TEXT,
                    PRESTADOR    TEXT,
                    AGREGADO     TEXT,
                    EMPRESA      TEXT
                     )", con);
                    sql1.ExecuteNonQuery();

                    // 2. Copia dados antigos
                    var sql2 = new SQLiteCommand(@"
                    INSERT INTO Veiculo_Nova 
                    (CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA)
                    SELECT CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA
                    FROM Veiculo", con);
                    sql2.ExecuteNonQuery();

                    // 3. Remove tabela antiga
                    var sql3 = new SQLiteCommand("DROP TABLE Veiculo", con);
                    sql3.ExecuteNonQuery();

                    // 4. Renomeia
                    var sql4 = new SQLiteCommand("ALTER TABLE Veiculo_Nova RENAME TO Veiculo", con);
                    sql4.ExecuteNonQuery();

                    MessageBox.Show("Coluna ID criada com sucesso!", "OK",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }

        }

        private void OcultarVisitas_CheckedChanged(object sender, EventArgs e)
        {
            var dt = (DataTable)ultimas_visitas.DataSource;

            if (OcultarVisitas.Checked)
            {
                dt.DefaultView.RowFilter = "SAIDA IS NULL OR SAIDA = ''";
            }
            else
            {
                dt.DefaultView.RowFilter = null;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}

