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

            string VeiculoDb = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "controleAcesso.db");

            string AgendamentoDb = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Agendamento.db");

            string Conexao = $"Data Source={VeiculoDb};Version=3;";

            string Conexaoagenda = $"Data Source={AgendamentoDb};Version=3;";

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


        private void txt_RG_TextChanged(object sender, EventArgs e)
        {
            string txt_RG = Console.ReadLine();
            TextBox txt = sender as TextBox;

            int pos = txt.SelectionStart; // salva posição do cursor
            txt.Text = txt.Text.ToUpper();
            txt.SelectionStart = pos;     // restaura a posição

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

        // Substitua o uso de SQLiteCommand para SQLiteConnection ao abrir a conexão.
        // Exemplo de correção para o método btn_Salvar:

        private void btn_Salvar(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                string sql = @"
                INSERT INTO Veiculo
                (CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA)
                VALUES
                (@CPF, @NOME, @CELULAR, @CPFAJUDANTE, @NOMEAJUDANTE, @DataHora, @PLACA, @TIPOVEICULO, @PRESTADOR, @AGREGADO, @EMPRESA)
                 ";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CPF", txt_RG.Text);
                    cmd.Parameters.AddWithValue("@NOME", txt_NOME.Text);
                    cmd.Parameters.AddWithValue("@CELULAR", txt_cel.Text);
                    cmd.Parameters.AddWithValue("@CPFAJUDANTE", txt_RG_A.Text);
                    cmd.Parameters.AddWithValue("@NOMEAJUDANTE", txt_NOME_A.Text);
                    cmd.Parameters.AddWithValue("@DataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@PLACA", txt_Placa.Text);
                    cmd.Parameters.AddWithValue("@TIPOVEICULO", TIPO.Text);
                    cmd.Parameters.AddWithValue("@PRESTADOR", PRESTADOR.Text);
                    cmd.Parameters.AddWithValue("@AGREGADO", AGREGADO.Text);
                    cmd.Parameters.AddWithValue("@EMPRESA", txt_OBS.Text.Trim());

                    cmd.ExecuteNonQuery();
                }

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

                MessageBox.Show("CADASTRADO!");

                txt_RG.Clear(); txt_NOME.Clear(); txt_cel.Clear(); txt_RG_A.Clear(); txt_NOME_A.Clear(); txt_Placa.Clear(); TIPO.Clear(); txt_OBS.Clear();
            }
        }
        // Faça o mesmo ajuste em outros métodos que usam SQLiteCommand para abrir conexão.
        // Exemplo para btn_visitas_Click:

        private void btn_visitas_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();

                    string sql = @"
                     SELECT CPF, NOME, CELULAR, CPFAJUDANTE AS 'CPF AJUDANTE', NOMEAJUDANTE AS 'NOME AJUDANTE', strftime('%d/%m/%Y %H:%M', DataHora) AS 'DATA / HORA',
                     PLACA, TIPOVEICULO AS 'TIPO VEICULO', PRESTADOR, AGREGADO, EMPRESA
                     FROM Veiculo
                     WHERE DATE(DataHora) = DATE('now')
                     ORDER BY DataHora DESC";

                    // Aqui você deve usar SQLiteCommand e SQLiteDataAdapter para preencher o DataTable
                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ultimas_visitas.DataSource = dt;
                        ultimas_visitas.ReadOnly = true;
                        ultimas_visitas.DefaultCellStyle.Font = new Font("Segoe UI", 12);
                        ultimas_visitas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                        ultimas_visitas.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                        ultimas_visitas.AlternatingRowsDefaultCellStyle = ultimas_visitas.DefaultCellStyle;
                        ultimas_visitas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
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

        }

        // Substitua todos os usos incorretos de "new SQLiteCommand(conexao)" por "new SQLiteConnection(conexao)"
        // e ajuste o uso de comandos conforme necessário.

        // Exemplo 1: btn_Salvar, btn_visitas_Click já estão corretos.

        // Exemplo 2: Corrija o método button1_Click:
        private void button1_Click(object sender, EventArgs e)
        {
            string placaProcurada = txt_Placa.Text.Trim().ToUpper();

            using (var con = new SQLiteConnection(conexao))
            {
                con.Open();
                string sql = @"
                    SELECT CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, EMPRESA
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
                            txt_RG.Clear();
                            txt_NOME.Clear();
                            txt_RG_A.Clear();
                            txt_NOME_A.Clear();
                            txt_OBS.Clear();
                            TIPO.Clear();
                            txt_cel.Clear();
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

        // Exemplo 3: Corrija o método btn_atualizar_Click:
        private void btn_atualizar_Click(object sender, EventArgs e)
        {
            string conexaoagenda = @"Data Source=ControleAcesso.db";

            DateTime hojeInicio = DateTime.Today;
            DateTime hojeFim = DateTime.Today.AddDays(1).AddSeconds(-1);

            string sql = @"
            SELECT USUARIO, NOME, EMPRESA,
            strftime('%d/%m/%Y %H:%M', DATAHORA) AS DATAHORA
            FROM AGENDAMENTO
            WHERE DataHora BETWEEN $inicio AND $fim
            ORDER BY DataHora DESC";

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
                MessageBox.Show("Informe placa ou RG para pesquisa.");
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
                                    strftime('%d/%m/%Y %H:%M', DataHora) AS 'DATA / HORA'
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
                                    strftime('%d/%m/%Y %H:%M', DataHora) AS 'DATA / HORA'
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

                        dt_historico.DataSource = dt;
                        dt_historico.ReadOnly = true;
                        dt_historico.DefaultCellStyle.Font = new Font("Segoe UI", 12);
                        dt_historico.ColumnHeadersDefaultCellStyle.Font =
                            new Font("Segoe UI", 12, FontStyle.Bold);
                        dt_historico.RowHeadersDefaultCellStyle.Font =
                            new Font("Segoe UI", 10, FontStyle.Bold);

                        if (dt.Rows.Count == 0)
                            MessageBox.Show("Nenhum registro encontrado.");
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
            string placaFiltro = txt_Placa.Text.Trim().ToUpper();
            string rgFiltro = txt_RG.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(placaFiltro) && string.IsNullOrEmpty(rgFiltro))
            {
                MessageBox.Show("Informe placa ou RG para pesquisa.");
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
                                    strftime('%d/%m/%Y %H:%M', DataHora) AS 'DATA / HORA'
                                FROM Veiculo
                                WHERE UPPER(Placa) = @placa
                                ORDER BY DataHora DESC";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@placa", placaFiltro);
                        }
                        else
                        {
                            sql = @"
                                SELECT 
                                    strftime('%d/%m/%Y %H:%M', DataHora) AS 'DATA / HORA'
                                FROM Veiculo
                                WHERE UPPER(CPF) = @CPF
                                ORDER BY DataHora DESC";

                            cmd.CommandText = sql;
                            cmd.Parameters.AddWithValue("@CPF", rgFiltro);
                        }

                        DataTable dt = new DataTable();
                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }

                        dt_historico.DataSource = dt;
                        dt_historico.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
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
    }
}
