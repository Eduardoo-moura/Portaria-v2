using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Portaria
{
    /// <summary>
    /// Chegada das mercadorias na portaria e confirmacao da retirada pelo
    /// destinatario.
    ///
    /// Cada linha da tabela MERCADORIA e uma chegada, no mesmo espirito da tabela
    /// VEICULO: nao ha cadastro de empresa nem de entregador, os dados sao
    /// gravados a cada entrega.
    /// </summary>
    public partial class Frm_Mercadoria : Form
    {
        // Mesma origem de dados usada pelo restante do sistema.
        private readonly string conexao = @"Data Source=ControleAcesso.db;";

        private static readonly Font FonteCelula = new Font("Segoe UI", 12);
        private static readonly Font FonteCabecalhoColuna = new Font("Segoe UI", 12, FontStyle.Bold);
        private static readonly Font FonteCabecalhoLinha = new Font("Segoe UI", 10, FontStyle.Bold);

        // Verde da linha ja entregue. A cor de selecao tambem muda: sem isso a
        // linha selecionada volta ao azul padrao e a marcacao some da vista.
        private static readonly Color FundoEntregue = Color.FromArgb(198, 239, 206);
        private static readonly Color TextoEntregue = Color.FromArgb(0, 97, 0);
        private static readonly Color FundoEntregueSelecionado = Color.FromArgb(112, 173, 71);
        private static readonly Color TextoEntregueSelecionado = Color.White;

        /// <summary>Valor gravado em MERCADORIA.ENTREGUE quando a retirada e confirmada.</summary>
        private const string MarcaEntregue = "SIM";

        /// <summary>Quantos dias a lista mostra, contando o de hoje.</summary>
        private const int DiasNaLista = 7;

        public Frm_Mercadoria()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.KeyPreview = true;

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

        private void Frm_Mercadoria_Load(object sender, EventArgs e)
        {
            // Quem recebe na portaria e sempre o porteiro logado: o campo so mostra.
            txt_Recebedor.Text = UsuarioLogado();

            dtg_mercadorias.ReadOnly = true;
            dtg_mercadorias.DefaultCellStyle.Font = FonteCelula;
            dtg_mercadorias.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoColuna;
            dtg_mercadorias.RowHeadersDefaultCellStyle.Font = FonteCabecalhoLinha;
            dtg_mercadorias.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dtg_mercadorias.MultiSelect = false;

            // Colunas no tamanho do conteudo: com o cabecalho so, DESTINATARIO e
            // EMPRESA saiam cortados.
            dtg_mercadorias.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            CarregarPeriodo();
            txt_Destinatario.Focus();
        }

        /// <summary>Nome do porteiro logado, ou vazio quando nao ha sessao.</summary>
        private static string UsuarioLogado()
        {
            return Sessao.Atual == null ? string.Empty : Sessao.Atual.NomeExibicao;
        }

        /// <summary>Somente o nivel 1 mexe numa entrega ja confirmada.</summary>
        private static bool PodeAlterarEntregaConfirmada()
        {
            return Sessao.Atual != null && Sessao.Atual.Nivel == Nivel.Total;
        }

        private void Btn_SalvarMerc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Destinatario.Text))
            {
                MessageBox.Show("O campo DESTINATÁRIO é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Destinatario.Focus();
                return;
            }

            try
            {
                using (var conn = new SQLiteConnection(conexao))
                {
                    conn.Open();
                    string sql = @"
                    INSERT INTO MERCADORIA
                    (DATAHORA, DESTINATARIO, EMPRESA, ENTREGADOR, RECEBEDOR, USUARIOREGISTRO, ENTREGUE)
                    VALUES
                    (@DATAHORA, @DESTINATARIO, @EMPRESA, @ENTREGADOR, @RECEBEDOR, @USUARIOREGISTRO, '')";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@DATAHORA", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@DESTINATARIO", txt_Destinatario.Text.Trim());
                        cmd.Parameters.AddWithValue("@EMPRESA", txt_Empresa.Text.Trim());
                        cmd.Parameters.AddWithValue("@ENTREGADOR", txt_Entregador.Text.Trim());
                        cmd.Parameters.AddWithValue("@RECEBEDOR", UsuarioLogado());

                        // Quem registrou a chegada, igual ao USUARIOENTRADA de VEICULO.
                        cmd.Parameters.AddWithValue("@USUARIOREGISTRO",
                            Sessao.Atual == null ? "" : Sessao.Atual.Login);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível gravar a mercadoria: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Conexao fechada antes do dialogo: nao mantem o banco travado
            // enquanto a mensagem estiver aberta na tela.
            MessageBox.Show("MERCADORIA REGISTRADA!");
            LimparCampos();
            CarregarPeriodo();
        }

        private void Btn_LimparMerc_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void Btn_AtualizarMerc_Click(object sender, EventArgs e)
        {
            CarregarPeriodo();
        }

        /// <summary>
        /// Relatorio das mercadorias por periodo, com escolha de colunas — o
        /// mesmo formato do relatorio personalizado de veiculos.
        /// </summary>
        private void Relatorio_personalizado_Click(object sender, EventArgs e)
        {
            using (var f = new Frm_relatorio_mercadoria())
            {
                f.ShowDialog(this);
            }
        }

        private void Btn_FecharMerc_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Duplo clique na grade faz o mesmo que o botao de confirmacao.</summary>
        private void dtg_mercadorias_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // clique no cabecalho
            ConfirmarEntrega();
        }

        private void Btn_Entregue_Click(object sender, EventArgs e)
        {
            ConfirmarEntrega();
        }

        /// <summary>
        /// Pergunta quem esta retirando a mercadoria selecionada e grava o nome
        /// com a data e a hora da retirada; a linha entao fica verde na lista.
        /// Numa mercadoria ja entregue, o botao desfaz a confirmacao.
        /// </summary>
        private void ConfirmarEntrega()
        {
            DataGridViewRow linha = dtg_mercadorias.CurrentRow;

            if (linha == null || linha.Cells["ID"].Value == null)
            {
                MessageBox.Show("Selecione na lista a mercadoria que está sendo retirada!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long id = Convert.ToInt64(linha.Cells["ID"].Value);

            if (EstaEntregue(linha))
            {
                DesfazerEntrega(id, linha);
                return;
            }

            string retirante;

            using (var dialogo = new Frm_RetiradaMercadoria(DescricaoDaLinha(linha), string.Empty))
            {
                if (dialogo.ShowDialog(this) != DialogResult.OK)
                    return;

                retirante = dialogo.NomeRetirante;
            }

            string sql = @"
            UPDATE MERCADORIA
            SET ENTREGUE = @ENTREGUE, RETIRADOPOR = @RETIRADOPOR,
                DATAENTREGA = @DATAENTREGA, USUARIOENTREGA = @USUARIOENTREGA
            WHERE ID = @ID";

            if (!Executar(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@ENTREGUE", MarcaEntregue);
                cmd.Parameters.AddWithValue("@RETIRADOPOR", retirante);
                cmd.Parameters.AddWithValue("@DATAENTREGA", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@USUARIOENTREGA", UsuarioLogado());
                cmd.Parameters.AddWithValue("@ID", id);
            }))
                return;

            // Recarrega e volta o cursor para a mesma mercadoria, para o porteiro
            // ver a linha mudar de cor sem procurar de novo na lista.
            CarregarPeriodo();
            Selecionar(id);
        }

        /// <summary>
        /// Desfaz uma entrega ja confirmada. So o nivel 1 pode: para o porteiro
        /// comum a retirada e definitiva, senao o registro de quem levou a
        /// mercadoria poderia ser apagado sem deixar rastro.
        /// </summary>
        private void DesfazerEntrega(long id, DataGridViewRow linha)
        {
            if (!PodeAlterarEntregaConfirmada())
            {
                MessageBox.Show(
                    "Esta mercadoria já foi entregue. Somente um usuário de nível 1 pode alterar a confirmação!",
                    "Acesso negado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string quem = Convert.ToString(linha.Cells["RETIRADO POR"].Value);

            DialogResult resposta = MessageBox.Show(
                "Esta mercadoria já foi retirada por " + quem + ". Deseja desfazer a confirmação?",
                "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resposta != DialogResult.Yes)
                return;

            string sql = @"
            UPDATE MERCADORIA
            SET ENTREGUE = '', RETIRADOPOR = '', DATAENTREGA = '', USUARIOENTREGA = ''
            WHERE ID = @ID";

            if (!Executar(sql, cmd => cmd.Parameters.AddWithValue("@ID", id)))
                return;

            CarregarPeriodo();
            Selecionar(id);
        }

        /// <summary>Identifica a mercadoria no dialogo de retirada.</summary>
        private static string DescricaoDaLinha(DataGridViewRow linha)
        {
            string destinatario = Convert.ToString(linha.Cells["DESTINATÁRIO"].Value);
            string empresa = Convert.ToString(linha.Cells["EMPRESA"].Value);

            if (string.IsNullOrWhiteSpace(empresa))
                return "MERCADORIA DE " + destinatario;

            return "MERCADORIA DE " + destinatario + Environment.NewLine + "ENTREGUE POR " + empresa;
        }

        /// <summary>
        /// Roda um UPDATE avisando na tela quando o banco recusa. Devolve false
        /// no erro, para o chamador nao seguir como se tivesse gravado.
        /// </summary>
        private bool Executar(string sql, Action<SQLiteCommand> parametros)
        {
            try
            {
                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();

                    using (var cmd = new SQLiteCommand(sql, con))
                    {
                        parametros(cmd);
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível alterar a entrega: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LimparCampos()
        {
            txt_Destinatario.Clear();
            txt_Empresa.Clear();
            txt_Entregador.Clear();

            // RECEBIDO POR nao entra: e sempre o usuario logado.
            txt_Destinatario.Focus();
        }

        /// <summary>
        /// Mercadorias que chegaram nos ultimos DiasNaLista dias, da mais recente
        /// para a mais antiga. A data e comparada em hora local: 'now' puro e UTC
        /// e trocaria de dia as 21h no horario de Brasilia.
        /// </summary>
        private void CarregarPeriodo()
        {
            try
            {
                var dt = new DataTable();

                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();
                    string sql = @"
                    SELECT ID,
                    strftime('%d/%m/%Y %H:%M', DATAHORA) AS 'CHEGADA',
                    DESTINATARIO AS 'DESTINATÁRIO',
                    EMPRESA,
                    ENTREGADOR,
                    RECEBEDOR AS 'RECEBIDO POR',
                    CASE WHEN IFNULL(ENTREGUE,'') = 'SIM' THEN 'ENTREGUE' ELSE 'NA PORTARIA' END AS 'SITUAÇÃO',
                    RETIRADOPOR AS 'RETIRADO POR',
                    strftime('%d/%m/%Y %H:%M', DATAENTREGA) AS 'DATA / HORA RETIRADA',
                    USUARIOENTREGA AS 'LIBERADO POR',
                    IFNULL(ENTREGUE,'') AS ENTREGUE
                    FROM MERCADORIA
                    WHERE DATE(DATAHORA) >= DATE('now', 'localtime', @DIAS)
                    ORDER BY DATAHORA DESC, ID DESC";

                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        // -6 dias mais o de hoje fecham os 7 dias da lista.
                        cmd.Parameters.AddWithValue("@DIAS", "-" + (DiasNaLista - 1) + " days");
                        da.Fill(dt);
                    }
                }

                dtg_mercadorias.DataSource = dt;

                // ID e ENTREGUE servem so para o codigo: um identifica a linha,
                // o outro decide a cor. A situacao ja aparece por extenso.
                OcultarColuna("ID");
                OcultarColuna("ENTREGUE");

                PintarEntregues();

                int entregues = 0;
                foreach (DataRow linha in dt.Rows)
                {
                    if (string.Equals(Convert.ToString(linha["ENTREGUE"]), MarcaEntregue,
                        StringComparison.OrdinalIgnoreCase))
                        entregues++;
                }

                lbl_lista.Text = string.Format(
                    "MERCADORIAS DOS ULTIMOS {0} DIAS ({1})   -   ENTREGUES: {2}   -   NA PORTARIA: {3}",
                    DiasNaLista, dt.Rows.Count, entregues, dt.Rows.Count - entregues);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void OcultarColuna(string nome)
        {
            if (dtg_mercadorias.Columns[nome] != null)
                dtg_mercadorias.Columns[nome].Visible = false;
        }

        /// <summary>Deixa em verde as linhas das mercadorias ja retiradas.</summary>
        private void PintarEntregues()
        {
            foreach (DataGridViewRow linha in dtg_mercadorias.Rows)
            {
                if (!EstaEntregue(linha))
                    continue;

                linha.DefaultCellStyle.BackColor = FundoEntregue;
                linha.DefaultCellStyle.ForeColor = TextoEntregue;
                linha.DefaultCellStyle.SelectionBackColor = FundoEntregueSelecionado;
                linha.DefaultCellStyle.SelectionForeColor = TextoEntregueSelecionado;
            }
        }

        private static bool EstaEntregue(DataGridViewRow linha)
        {
            DataGridViewCell celula = linha.Cells["ENTREGUE"];

            return celula != null
                && string.Equals(Convert.ToString(celula.Value), MarcaEntregue,
                    StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Deixa o cursor da grade na mercadoria de um ID, se ela estiver na lista.</summary>
        private void Selecionar(long id)
        {
            foreach (DataGridViewRow linha in dtg_mercadorias.Rows)
            {
                if (linha.Cells["ID"].Value == null || Convert.ToInt64(linha.Cells["ID"].Value) != id)
                    continue;

                dtg_mercadorias.CurrentCell = linha.Cells[dtg_mercadorias.FirstDisplayedScrollingColumnIndex];
                return;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
