using QuestPDF.Infrastructure;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Portaria
{
    internal static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Estrutura do banco (colunas ID/SAIDA em VEICULO) e o cadastro de
            // usuarios com o admin inicial (admin / admin).
            try
            {
                Banco.GarantirEstrutura();
                Usuarios.GarantirEstrutura();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível preparar o banco de dados: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Sem login validado, o sistema nao abre.
            using (var login = new Frm_Login())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;
            }

            Application.Run(new Frm_Veiculo());
        }
        public class conexao
        {
            private SQLiteConnection con = new SQLiteConnection(
                @"Data Source=ControleAcesso.db;Version=3;");

            public SQLiteConnection Abrir()
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                return con;
            }
            public void Fechar()
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

    }
}
