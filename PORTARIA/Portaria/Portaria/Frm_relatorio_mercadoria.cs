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
    /// <summary>
    /// Relatorio das mercadorias em que o usuario escolhe, num checklist, quais
    /// campos saem no PDF. Mesma mecanica do relatorio personalizado de veiculos,
    /// aplicada a tabela MERCADORIA.
    /// </summary>
    public partial class Frm_relatorio_mercadoria : Form
    {
        /// <summary>Um campo disponivel no checklist: o titulo da coluna e a expressao SQL.</summary>
        private class Campo
        {
            public readonly string Titulo;
            public readonly string Sql;

            public Campo(string titulo, string sql)
            {
                Titulo = titulo;
                Sql = sql;
            }

            // O CheckedListBox exibe o resultado de ToString().
            public override string ToString()
            {
                return Titulo;
            }
        }

        /// <summary>
        /// Campos oferecidos no checklist. As expressoes SQL sao fixas aqui no
        /// codigo — nada vem digitado pelo usuario.
        /// </summary>
        private static readonly Campo[] CamposDisponiveis =
        {
            new Campo("CHEGADA",              "IFNULL(strftime('%d/%m/%Y %H:%M', DATAHORA),'')"),
            new Campo("DESTINATARIO",         "IFNULL(DESTINATARIO,'')"),
            new Campo("EMPRESA",              "IFNULL(EMPRESA,'')"),
            new Campo("ENTREGADOR",           "IFNULL(ENTREGADOR,'')"),
            new Campo("RECEBIDO POR",         "IFNULL(RECEBEDOR,'')"),
            new Campo("SITUACAO",             "CASE WHEN IFNULL(ENTREGUE,'') = 'SIM' THEN 'ENTREGUE' ELSE 'NA PORTARIA' END"),
            new Campo("RETIRADO POR",         "IFNULL(RETIRADOPOR,'')"),
            new Campo("DATA / HORA RETIRADA", "IFNULL(strftime('%d/%m/%Y %H:%M', DATAENTREGA),'')"),
            new Campo("LIBERADO POR",         "IFNULL(USUARIOENTREGA,'')"),
            new Campo("USUARIO DA CHEGADA",   "IFNULL(USUARIOREGISTRO,'')")
        };

        /// <summary>Item do combo de usuarios: o texto exibido e o login usado no filtro.</summary>
        private class ItemUsuario
        {
            public readonly string Texto;
            public readonly string Login;

            public ItemUsuario(string texto, string login)
            {
                Texto = texto;
                Login = login;
            }

            public override string ToString()
            {
                return Texto;
            }
        }

        // Logins especiais do combo: qualquer usuario e registros gravados sem
        // usuario nenhum.
        private const string TodosOsUsuarios = null;
        private const string SemUsuario = " SEM";

        private const string Conexao = @"Data Source=controleAcesso.db;Version=3;";

        public Frm_relatorio_mercadoria()
        {
            InitializeComponent();
        }

        private void Frm_relatorio_mercadoria_Load(object sender, EventArgs e)
        {
            foreach (Campo campo in CamposDisponiveis)
                clb_campos.Items.Add(campo, true); // começa tudo marcado

            // Padrao: do primeiro dia do mes ate hoje.
            DateTime hoje = DateTime.Today;
            Data_inicio.Value = new DateTime(hoje.Year, hoje.Month, 1);
            data_final.Value = hoje;

            CarregarUsuarios();
        }

        /// <summary>
        /// Monta o combo com "TODOS", os usuarios cadastrados e tambem os logins
        /// que aparecem nas chegadas mas nao estao mais no cadastro.
        /// </summary>
        private void CarregarUsuarios()
        {
            cmb_usuario.Items.Add(new ItemUsuario("TODOS OS USUARIOS", TodosOsUsuarios));

            var jaAdicionados = new List<string>();

            try
            {
                using (var con = new SQLiteConnection(Conexao))
                {
                    con.Open();

                    string sql = @"
                        SELECT LOGIN, IFNULL(NOME,'') AS NOME
                        FROM USUARIO
                        ORDER BY LOGIN";

                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string login = dr["LOGIN"].ToString().Trim();
                            if (login.Length == 0) continue;

                            string nome = dr["NOME"].ToString().Trim();
                            string texto = nome.Length == 0 ? login : login + " - " + nome;

                            cmb_usuario.Items.Add(new ItemUsuario(texto, login));
                            jaAdicionados.Add(login.ToUpperInvariant());
                        }
                    }

                    // Logins presentes nas chegadas que nao estao (ou nao estao
                    // mais) no cadastro de usuarios.
                    sql = @"
                        SELECT DISTINCT TRIM(USUARIOREGISTRO) AS LOGIN
                        FROM MERCADORIA
                        WHERE TRIM(IFNULL(USUARIOREGISTRO,'')) <> ''
                        ORDER BY 1";

                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string login = dr["LOGIN"].ToString().Trim();

                            if (login.Length == 0) continue;
                            if (jaAdicionados.Contains(login.ToUpperInvariant())) continue;

                            cmb_usuario.Items.Add(new ItemUsuario(login + " (REMOVIDO)", login));
                            jaAdicionados.Add(login.ToUpperInvariant());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar a lista de usuários: " + ex.Message,
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            cmb_usuario.Items.Add(new ItemUsuario("(SEM USUARIO REGISTRADO)", SemUsuario));
            cmb_usuario.SelectedIndex = 0;
        }

        /// <summary>Login escolhido no combo; TodosOsUsuarios quando nao ha filtro.</summary>
        private string UsuarioSelecionado
        {
            get
            {
                ItemUsuario item = cmb_usuario.SelectedItem as ItemUsuario;
                return item == null ? TodosOsUsuarios : item.Login;
            }
        }

        private void btn_marcar_Click(object sender, EventArgs e)
        {
            DefinirTodos(true);
        }

        private void btn_desmarcar_Click(object sender, EventArgs e)
        {
            DefinirTodos(false);
        }

        private void DefinirTodos(bool marcado)
        {
            for (int i = 0; i < clb_campos.Items.Count; i++)
                clb_campos.SetItemChecked(i, marcado);
        }

        private void btn_gerar_Click(object sender, EventArgs e)
        {
            List<Campo> escolhidos = new List<Campo>();
            foreach (object item in clb_campos.CheckedItems)
                escolhidos.Add((Campo)item);

            if (escolhidos.Count == 0)
            {
                MessageBox.Show("Marque pelo menos um campo para o relatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime inicio = Data_inicio.Value.Date;
            DateTime fimExibicao = data_final.Value.Date;

            if (fimExibicao < inicio)
            {
                MessageBox.Show("A data final não pode ser menor que a data inicial!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime fim = fimExibicao.AddDays(1); // limite superior exclusivo
            string usuario = UsuarioSelecionado;

            List<string[]> dados;

            Cursor anterior = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                dados = Consultar(escolhidos, inicio, fim, usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao consultar banco: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = anterior;
            }

            if (dados.Count == 0)
            {
                MessageBox.Show(usuario == TodosOsUsuarios
                        ? "Nenhuma mercadoria encontrada no período informado."
                        : "Nenhuma mercadoria encontrada no período para o usuário selecionado.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string pdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "relatorio_mercadorias.pdf");

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

            try
            {
                GerarPdf(pdfPath, escolhidos, dados, inicio, fimExibicao, cmb_usuario.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o PDF: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Caminho entre aspas: a pasta do sistema pode ter espacos no nome.
            System.Diagnostics.Process.Start("explorer.exe", "\"" + pdfPath + "\"");
        }

        private List<string[]> Consultar(List<Campo> campos, DateTime inicio, DateTime fim, string usuario)
        {
            string[] expressoes = new string[campos.Count];
            for (int i = 0; i < campos.Count; i++)
                expressoes[i] = campos[i].Sql;

            string filtroEntrega = chk_sem_entrega.Checked
                ? " AND IFNULL(ENTREGUE,'') <> 'SIM'"
                : "";

            // Quem registrou a chegada. O login vai por parametro; aqui so entra
            // o trecho fixo da condicao.
            string filtroUsuario = "";
            if (usuario == SemUsuario)
                filtroUsuario = " AND TRIM(IFNULL(USUARIOREGISTRO,'')) = ''";
            else if (usuario != TodosOsUsuarios)
                filtroUsuario = " AND UPPER(TRIM(IFNULL(USUARIOREGISTRO,''))) = UPPER(@USU)";

            string sql = string.Format(@"
                SELECT {0}
                FROM MERCADORIA
                WHERE datetime(DATAHORA) >= datetime(@INI)
                  AND datetime(DATAHORA) <  datetime(@FIM){1}{2}
                ORDER BY datetime(DATAHORA)",
                string.Join(", ", expressoes), filtroEntrega, filtroUsuario);

            List<string[]> linhas = new List<string[]>();

            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.Add("@INI", DbType.String).Value = inicio.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add("@FIM", DbType.String).Value = fim.ToString("yyyy-MM-dd HH:mm:ss");

                    if (filtroUsuario.Contains("@USU"))
                        cmd.Parameters.Add("@USU", DbType.String).Value = usuario.Trim();

                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            string[] linha = new string[campos.Count];
                            for (int i = 0; i < campos.Count; i++)
                                linha[i] = dr.IsDBNull(i) ? "" : dr.GetValue(i).ToString();

                            linhas.Add(linha);
                        }
                    }
                }
            }

            return linhas;
        }

        private static void GerarPdf(string pdfPath, List<Campo> campos, List<string[]> dados,
            DateTime inicio, DateTime fim, string usuarioDescricao)
        {
            int totalRegistros = dados.Count;

            Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    // Muitas colunas nao cabem em pe: vira a folha.
                    page.Size(campos.Count > 6 ? PageSizes.A4.Landscape() : PageSizes.A4);
                    page.Margin(10);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(8));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("RELATÓRIO DE MERCADORIAS").FontSize(12).SemiBold();
                        col.Item().Text($"Período de {inicio:dd/MM/yyyy} a {fim:dd/MM/yyyy}").FontSize(8);
                        col.Item().Text($"Usuário da chegada: {usuarioDescricao}").FontSize(8);
                    });

                    page.Content().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(col =>
                        {
                            foreach (Campo campo in campos)
                                col.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            foreach (Campo campo in campos)
                                header.Cell().Element(CellHeader).Text(campo.Titulo).SemiBold();
                        });

                        foreach (string[] linha in dados)
                        {
                            foreach (string valor in linha)
                                table.Cell().Element(CellBody).Text(valor);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Total de mercadorias: {totalRegistros}  |  Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                });
            })
            .GeneratePdf(pdfPath);
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
    }
}
