using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace Portaria
{
    public partial class Frm_relatorio_data : Form
    {
        private const int TotalColunas = 10;

        public Frm_relatorio_data()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false; // opcional

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
                    SELECT CPF, NOME, CELULAR, CPFAJUDANTE , NOMEAJUDANTE, DATAHORA, SAIDA,
                    PLACA, PRESTADOR, EMPRESA
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

            // Colunas resolvidas uma vez fora do laco, em vez de por nome a cada linha.
            DataColumn colCpf = dt.Columns["CPF"];
            DataColumn colNome = dt.Columns["NOME"];
            DataColumn colCelular = dt.Columns["CELULAR"];
            DataColumn colCpfAjudante = dt.Columns["CPFAJUDANTE"];
            DataColumn colNomeAjudante = dt.Columns["NOMEAJUDANTE"];
            DataColumn colDataHora = dt.Columns["DataHora"];
            DataColumn colSaida = dt.Columns["SAIDA"];
            DataColumn colPlaca = dt.Columns["PLACA"];
            DataColumn colPrestador = dt.Columns["PRESTADOR"];
            DataColumn colEmpresa = dt.Columns["EMPRESA"];

            List<string[]> dadosFiltrados = new List<string[]>(dt.Rows.Count);

            foreach (DataRow row in dt.Rows)
            {
                DateTime DataHora;
                string dataFormatada = "";

                if (DateTime.TryParse(row[colDataHora]?.ToString(), out DataHora))
                    dataFormatada = DataHora.ToString("dd/MM/yyyy HH:mm");

                dadosFiltrados.Add(new string[TotalColunas]
                {
                row[colCpf]?.ToString() ?? "",
                row[colNome]?.ToString() ?? "",
                row[colCelular]?.ToString() ?? "",
                row[colCpfAjudante]?.ToString() ?? "",
                row[colNomeAjudante]?.ToString() ?? "",
                dataFormatada,
                row[colSaida]?.ToString() ?? "",
                row[colPlaca]?.ToString() ?? "",
                row[colPrestador]?.ToString() ?? "",
                //row["AGREGADO"]?.ToString() ?? "",
                row[colEmpresa]?.ToString() ?? ""
                });
            }

            string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatorio.pdf");

            try
            {
                if (File.Exists(pdfPath))
                    File.Delete(pdfPath);
            }
            catch (IOException)
            {
                MessageBox.Show("Feche o relatório que já está aberto e gere novamente.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                            for (int i = 0; i < TotalColunas; i++)
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
                            header.Cell().Element(CellHeader).Text("ENTRADA").SemiBold();
                            header.Cell().Element(CellHeader).Text("SAÍDA").SemiBold();
                            header.Cell().Element(CellHeader).Text("PLACA").SemiBold();
                            header.Cell().Element(CellHeader).Text("PRESTADOR").SemiBold();
                            //header.Cell().Element(CellHeader).Text("AGREGADO").SemiBold();
                            header.Cell().Element(CellHeader).Text("EMPRESA").SemiBold();
                        });

                        // Corpo
                        foreach (var linha in dadosFiltrados)
                        {
                            for (int i = 0; i < TotalColunas; i++)
                                table.Cell().Element(CellBody).Text(i < linha.Length ? linha[i] : "");
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Total de visitas: {totalRegistros}  |  Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            })
            .GeneratePdf(pdfPath);

            // Caminho entre aspas: a pasta do sistema tem espacos no nome e o
            // explorer.exe abriria o argumento errado sem elas.
            System.Diagnostics.Process.Start("explorer.exe", "\"" + pdfPath + "\"");

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
