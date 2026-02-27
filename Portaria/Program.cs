using Microsoft.Data.Sqlite;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
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
            Application.Run(new Frm_Veiculo());
        }
        public class Conexao
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
