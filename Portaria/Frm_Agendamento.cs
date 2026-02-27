using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portaria
{
    public partial class Frm_Agendamento : Form
    {
        public Frm_Agendamento()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true; // opcional

            string conexaoangeda = @"Data Source=ControleAcesso.db;Version=3;";

            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.CharacterCasing = CharacterCasing.Upper;
                }
            }

        }

       private readonly string conexaoagenda =
       @"Data Source=ControleAcesso.db;Version=3;";




        private void Frm_Agendamento_Load(object sender, EventArgs e)
        {
             string usuario = Environment.UserName;
             lbl_user.Text = usuario;    
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string NOME = Console.ReadLine();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string EMPRESA = Console.ReadLine();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            string data_hora = Console.ReadLine();
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

                agenda.ReadOnly = true;
                agenda.DefaultCellStyle.Font = new Font("Segoe UI", 12);
                agenda.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                agenda.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10);
        }
        
        
           
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(conexaoagenda))
                {
                    con.Open();

                    string sql = @"
                    INSERT INTO AGENDAMENTO (USUARIO, NOME, EMPRESA, DATAHORA)
                    VALUES (@USUARIO, @NOME, @EMPRESA, @DATAHORA)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        DateTime DataHora;
                        if (!DateTime.TryParse(txt_data.Text, out DataHora))
                        {
                            MessageBox.Show("Data inválida!");
                            return;
                        }

                        cmd.Parameters.AddWithValue("@USUARIO", Environment.UserName);
                        cmd.Parameters.AddWithValue("@NOME", txt_nome.Text.Trim());
                        cmd.Parameters.AddWithValue("@EMPRESA", txt_emp.Text.Trim());
                        cmd.Parameters.AddWithValue("@DATAHORA", DataHora.ToString("yyyy-MM-dd HH:mm:ss"));

                        cmd.ExecuteNonQuery();
                    }

                    txt_nome.Clear();
                    txt_emp.Clear();
                    MessageBox.Show("Agendamento realizado com sucesso!");


                }

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
