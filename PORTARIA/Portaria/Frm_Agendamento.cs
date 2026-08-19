using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Portaria
{
    public partial class Frm_Agendamento : Form
    {
        // Fontes criadas uma unica vez, em vez de a cada atualizacao da grid.
        private static readonly Font FonteCelula = new Font("Segoe UI", 12);
        private static readonly Font FonteCabecalhoColuna = new Font("Segoe UI", 12, FontStyle.Bold);
        private static readonly Font FonteCabecalhoLinha = new Font("Segoe UI", 10);

        public Frm_Agendamento()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true; // opcional

            AplicarMaiusculas(this);
        }

        private static void AplicarMaiusculas(Control raiz)
        {
            foreach (Control c in raiz.Controls)
            {
                if (c is TextBox txt)
                    txt.CharacterCasing = CharacterCasing.Upper;
                else if (c.HasChildren)
                    AplicarMaiusculas(c);
            }
        }

       private readonly string conexaoagenda =
       @"Data Source=ControleAcesso.db;Version=3;";




        private void Frm_Agendamento_Load(object sender, EventArgs e)
        {
             string usuario = Environment.UserName;
             lbl_user.Text = usuario;

            // Estilo aplicado uma vez: sobrevive as trocas de DataSource.
            agenda.ReadOnly = true;
            agenda.DefaultCellStyle.Font = FonteCelula;
            agenda.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoColuna;
            agenda.RowHeadersDefaultCellStyle.Font = FonteCabecalhoLinha;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void agenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = @"
            SELECT NOME, EMPRESA,strftime('%d/%m/%Y %H:%M', DATAHORA) AS DATAHORA
            FROM AGENDAMENTO
            WHERE USUARIO = $USUARIO
            ORDER BY DataHora DESC";

            DataTable dt = new DataTable();

            using (SQLiteConnection con = new SQLiteConnection(conexaoagenda))
            {
                con.Open();

                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("$USUARIO", Environment.UserName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }

            agenda.DataSource = dt;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime DataHora;
                if (!DateTime.TryParse(txt_data.Text, out DataHora))
                {
                    MessageBox.Show("Data inválida!");
                    return;
                }

                using (SQLiteConnection con = new SQLiteConnection(conexaoagenda))
                {
                    con.Open();

                    string sql = @"
                    INSERT INTO AGENDAMENTO (USUARIO, NOME, EMPRESA, DATAHORA)
                    VALUES (@USUARIO, @NOME, @EMPRESA, @DATAHORA)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@USUARIO", Environment.UserName);
                        cmd.Parameters.AddWithValue("@NOME", txt_nome.Text.Trim());
                        cmd.Parameters.AddWithValue("@EMPRESA", txt_emp.Text.Trim());
                        cmd.Parameters.AddWithValue("@DATAHORA", DataHora.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }
                }

                // Conexao fechada antes do dialogo: nao trava o banco enquanto
                // a mensagem estiver aberta.
                txt_nome.Clear();
                txt_emp.Clear();
                MessageBox.Show("Agendamento realizado com sucesso!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void tempo_Tick(object sender, EventArgs e)
        {
            btn_atualizar.PerformClick(); // auto-clique
        }

        private void lbl_NOME_Click(object sender, EventArgs e)
        {

        }
    }
}
