using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Data.Sqlite;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Portaria
{
    public partial class Frm_relatorio_data : Form
    {

        public Frm_relatorio_data()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false; // opcional

            foreach (Control c in this.Controls)
            {
                if (c is TextBox txt)
                {
                    txt.CharacterCasing = CharacterCasing.Upper;
                }
            }


        }

        private static QuestPDF.Infrastructure.IContainer CellHeader(QuestPDF.Infrastructure.IContainer container) => container
            .Padding(1)
            .Background("#E0E0E0")
            .Border(0)
            .BorderColor("#000")
            .AlignMiddle();

        private static QuestPDF.Infrastructure.IContainer CellBody(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Padding(1)
                .MinHeight(2)
                .BorderBottom(1)
                .BorderColor("#CCC")
                .AlignMiddle();
        }

        private void btn_gerar_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            try
            {
                string conexao = @"Data Source=controleAcesso.db;Version=3;";

                DateTime inicio = Data_inicio.Value.Date;
                DateTime fim = data_final.Value.Date.AddDays(1);

                using (SQLiteConnection con = new SQLiteConnection(conexao))
                {
                    con.Open(); // 🔴 OBRIGATÓRIO

                    string sql = @"
                    SELECT CPF, NOME, CELULAR, CPFAJUDANTE , NOMEAJUDANTE, DATAHORA,
                    PLACA, PRESTADOR, AGREGADO, EMPRESA
                    FROM VEICULO
                    WHERE datetime(DATAHORA) >= datetime(@INI)
                    AND datetime(DATAHORA) < datetime(@FIM)
                    ORDER BY datetime(DATAHORA)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.Add("@INI", DbType.String).Value =
                        inicio.ToString("yyyy-MM-dd HH:mm:ss");

                        cmd.Parameters.Add("@FIM", DbType.String).Value =
                        fim.ToString("yyyy-MM-dd HH:mm:ss");

                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao consultar banco: " + ex.Message);
                return;
            }

            // O restante do método permanece igual
            List<string[]> dadosFiltrados = new List<string[]>();

            foreach (DataRow row in dt.Rows)
            {
                DateTime DataHora;
                string dataFormatada = "";

                if (DateTime.TryParse(row["DataHora"]?.ToString(), out DataHora))
                    dataFormatada = DataHora.ToString("dd/MM/yyyy HH:mm");

                dadosFiltrados.Add(new string[]
                {
                row["CPF"]?.ToString() ?? "",
                row["NOME"]?.ToString() ?? "",
                row["CELULAR"]?.ToString() ?? "",
                row["CPFAJUDANTE"]?.ToString() ?? "",
                row["NOMEAJUDANTE"]?.ToString() ?? "",
                dataFormatada,
                row["PLACA"]?.ToString() ?? "",
                row["PRESTADOR"]?.ToString() ?? "",
                row["AGREGADO"]?.ToString() ?? "",
                row["EMPRESA"]?.ToString() ?? ""
                });
            }
        
            string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatorio.pdf");

            if (File.Exists(pdfPath))
                File.Delete(pdfPath);

            Document.Create(doc =>
            {
                int totalRegistros = dadosFiltrados.Count;

                doc.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1);
                    page.PageColor(Colors.White);

                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8));

                    page.Content().Padding(2).Table(table =>
                    {
                        // Criar 10 colunas
                        table.ColumnsDefinition(col =>
                        {
                            for (int i = 0; i < 10; i++)
                                col.RelativeColumn();
                        });

                        // Cabeçalho
                        table.Header(header =>
                        {
                            header.Cell().Element(CellHeader).Text("CPF").SemiBold();
                            header.Cell().Element(CellHeader).Text("NOME MOTORISTA").SemiBold();
                            header.Cell().Element(CellHeader).Text("CELULAR").SemiBold();
                            header.Cell().Element(CellHeader).Text("CPF AJUDANTE").SemiBold();
                            header.Cell().Element(CellHeader).Text("NOME AJUDANTE").SemiBold();
                            header.Cell().Element(CellHeader).Text("DATA/HORA").SemiBold();
                            header.Cell().Element(CellHeader).Text("PLACA").SemiBold();
                            header.Cell().Element(CellHeader).Text("PRESTADOR").SemiBold();
                            header.Cell().Element(CellHeader).Text("AGREGADO").SemiBold();
                            header.Cell().Element(CellHeader).Text("EMPRESA").SemiBold();
                        });

                        // Corpo
                        foreach (var linha in dadosFiltrados)
                        {
                            var safe = new string[10];

                            for (int i = 0; i < 10; i++)
                                safe[i] = (i < linha.Length) ? linha[i] : "";

                            for (int i = 0; i < 10; i++)
                                table.Cell().Element(CellBody).Text(safe[i]);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Total de visitas: {totalRegistros}  |  Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            })
            .GeneratePdf(pdfPath);

            System.Diagnostics.Process.Start("explorer.exe", pdfPath);

        }

        private void lbl_data_inicio_Click(object sender, EventArgs e)
        {

        }

        private void Frm_relatorio_data_Load(object sender, EventArgs e)
        {

        }

        private void Data_inicio_ValueChanged(object sender, EventArgs e)
        {

        }
    }
    
}