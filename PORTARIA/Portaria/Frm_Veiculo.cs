using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;
using static Portaria.Program;

namespace Portaria
{
    public partial class Frm_Veiculo : Form
    {
        // Fontes criadas uma unica vez. Antes eram alocadas (e nunca liberadas)
        // a cada atualizacao de cada grid, vazando handles GDI.
        private static readonly Font FonteCelula = new Font("Segoe UI", 12);
        private static readonly Font FonteCabecalhoColuna = new Font("Segoe UI", 12, FontStyle.Bold);
        private static readonly Font FonteCabecalhoLinha = new Font("Segoe UI", 10, FontStyle.Bold);
        private static readonly Font FonteUsuarioLogado = new Font("Segoe UI", 9, FontStyle.Bold);

        // Visualizacao do "AGENDAMENTO DO DIA" desativada. Para voltar a exibir
        // a grade, basta trocar para true: a consulta e o codigo continuam aqui.
        private static readonly bool MostrarAgendamentoDoDia = false;

        private readonly string conexao =
        @"Data Source=ControleAcesso.db;";

        private readonly string conexaoagenda =
        @"Data Source=ControleAcesso.db;";

        // PLACA so com letras e numeros, em maiusculas. Sem isso a busca por
        // "GCT6604" nao acha os registros gravados como "GCT 6604".
        private const string PlacaNormalizada =
            @"REPLACE(REPLACE(REPLACE(REPLACE(UPPER(IFNULL(PLACA,'')),' ',''),'-',''),'/',''),'.','')";

        public Frm_Veiculo()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.MinimizeBox = true; // opcional
            this.KeyPreview = true;

            // Maiusculas pela propriedade do proprio controle (inclusive nos que
            // estao dentro de GroupBox/TabPage), em vez de reescrever o texto a
            // cada tecla digitada.
            AplicarMaiusculas(this);

            // Mascara da placa e conferencia do CPF ja na digitacao.
            Mascaras.AplicarPlaca(txt_Placa);
            Mascaras.AplicarDocumento(txt_RG);
            Mascaras.AplicarDocumento(txt_RG_A);
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

        private void Frm_Veiculo_Load(object sender, EventArgs e)
        {
            AplicarUsuarioLogado();

            // Estilo das grids aplicado uma vez: sobrevive as trocas de DataSource,
            // portanto nao precisa ser refeito em cada consulta.
            ConfigurarGrid(ultimas_visitas);
            ultimas_visitas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            ultimas_visitas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            ConfigurarGrid(dt_historico);

            // Agendamento do dia: grade e titulo fora da tela.
            dtg_agendamento.Visible = MostrarAgendamentoDoDia;
            Txt_agendamento.Visible = MostrarAgendamentoDoDia;

            if (MostrarAgendamentoDoDia)
                ConfigurarGrid(dtg_agendamento);
        }

        /// <summary>
        /// Mostra o usuario logado na barra de menu e libera o cadastro de
        /// usuarios apenas para o nivel 1.
        /// </summary>
        private void AplicarUsuarioLogado()
        {
            UsuarioInfo usuario = Sessao.Atual;

            if (usuario == null)
            {
                lbl_usuario_logado.Text = string.Empty;
                Strip_usuarios.Visible = false;
                return;
            }

            lbl_usuario_logado.Font = FonteUsuarioLogado;
            lbl_usuario_logado.Text = "USUARIO: " + usuario.NomeExibicao;
            Strip_usuarios.Visible = usuario.PodeCadastrarUsuario;
        }

        private void Usuarios_cadastrar_Click(object sender, EventArgs e)
        {
            // Segunda barreira: o menu fica oculto para o nivel 2, mas a
            // verificacao tambem e feita aqui.
            if (Sessao.Atual == null || !Sessao.Atual.PodeCadastrarUsuario)
            {
                MessageBox.Show("Seu nível de acesso não permite cadastrar usuários!", "Acesso negado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (Frm_Usuarios f = new Frm_Usuarios())
            {
                f.ShowDialog(this);
            }
        }

        private static void ConfigurarGrid(DataGridView grid)
        {
            grid.ReadOnly = true;
            grid.DefaultCellStyle.Font = FonteCelula;
            grid.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoColuna;
            grid.RowHeadersDefaultCellStyle.Font = FonteCabecalhoLinha;
            grid.AlternatingRowsDefaultCellStyle = grid.DefaultCellStyle;
        }

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
            AcceptButton = btn_rg;
        }
        private void txt_NOME_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            AcceptButton = button1;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
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

            // Segunda barreira do CPF: pega o numero que entrou colado ou que veio
            // de um registro antigo do banco. Mesma regra para motorista e ajudantes.
            if (!DocumentoAceito(txt_RG, "RG / CPF")) return;
            if (!DocumentosDosAjudantesAceitos()) return;

            // A placa vai sem separador quando forma uma placa completa; textos
            // como "S/ PLACA" continuam como o porteiro digitou.
            string placaMascarada = Placa.Aplicar(txt_Placa.Text);
            string placa = Placa.Completa(placaMascarada) ? placaMascarada : txt_Placa.Text.Trim();

            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                string sql = @"
                INSERT INTO Veiculo
                (CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA, USUARIOENTRADA)
                VALUES
                (@CPF, @NOME, @CELULAR, @CPFAJUDANTE, @NOMEAJUDANTE, @DataHora, @SAIDA, @PLACA, @TIPOVEICULO, @PRESTADOR, @AGREGADO, @EMPRESA, @USUARIOENTRADA)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CPF", txt_RG.Text);
                    cmd.Parameters.AddWithValue("@NOME", txt_NOME.Text);
                    cmd.Parameters.AddWithValue("@CELULAR", txt_cel.Text);
                    cmd.Parameters.AddWithValue("@CPFAJUDANTE", txt_RG_A.Text);
                    cmd.Parameters.AddWithValue("@NOMEAJUDANTE", txt_NOME_A.Text);
                    cmd.Parameters.AddWithValue("@DataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@SAIDA", "");
                    cmd.Parameters.AddWithValue("@PLACA", placa);
                    cmd.Parameters.AddWithValue("@TIPOVEICULO", TIPO.Text);
                    cmd.Parameters.AddWithValue("@PRESTADOR", PRESTADOR.Text);
                    cmd.Parameters.AddWithValue("@AGREGADO", AGREGADO.Text);
                    cmd.Parameters.AddWithValue("@EMPRESA", txt_OBS.Text.Trim());

                    // Quem registrou a entrada: usado no filtro do relatorio personalizado.
                    cmd.Parameters.AddWithValue("@USUARIOENTRADA",
                        Sessao.Atual == null ? "" : Sessao.Atual.Login);

                    cmd.ExecuteNonQuery();
                }
            }

            // Conexao fechada antes do dialogo: nao mantem o banco travado
            // enquanto a mensagem estiver aberta na tela.
            MessageBox.Show("CADASTRADO!");
            LimparCampo();

            btn_visitas.PerformClick(); // ← recarrega o DataGrid
        }
        /// <summary>
        /// Passa a regra do CPF do motorista em todas as abas de ajudante,
        /// inclusive nas criadas em tempo de execucao.
        /// </summary>
        private bool DocumentosDosAjudantesAceitos()
        {
            foreach (TabPage aba in Tab_Ajudantes.TabPages)
            {
                foreach (Control ctrl in aba.Controls)
                {
                    TextBox campo = ctrl as TextBox;

                    if (campo == null || !CampoDeDocumento(campo))
                        continue;

                    if (DocumentoAceito(campo, "RG / CPF de " + aba.Text))
                        continue;

                    Tab_Ajudantes.SelectedTab = aba; // mostra a aba onde esta o erro
                    campo.Focus();
                    campo.SelectAll();
                    return false;
                }
            }

            return true;
        }

        /// <summary>Campos de documento das abas: o da aba fixa e os das abas novas.</summary>
        private static bool CampoDeDocumento(TextBox campo)
        {
            return campo.Name == "txt_RG_A"
                || campo.Name.StartsWith("txtRgAjudante", StringComparison.Ordinal);
        }

        /// <summary>
        /// Recusa o campo quando o documento tem 11 digitos (portanto e CPF) e o
        /// digito verificador nao fecha. RG e documento em branco passam direto.
        /// </summary>
        private static bool DocumentoAceito(TextBox campo, string descricao)
        {
            string texto = campo.Text.Trim();

            if (!Documento.EhCpf(texto) || Documento.CpfValido(texto))
                return true;

            MessageBox.Show("O CPF informado em " + descricao + " é inválido: o dígito verificador não confere.",
                "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            campo.Focus();
            campo.SelectAll();
            return false;
        }

        private void btn_visitas_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

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
                        da.Fill(dt);
                    }
                }

                ultimas_visitas.DataSource = dt;

                // Oculta ID somente se a coluna existir
                if (ultimas_visitas.Columns["ID"] != null)
                    ultimas_visitas.Columns["ID"].Visible = false;

                // Comportamento preservado: o filtro volta marcado a cada atualizacao.
                OcultarVisitas.Checked = true;
                AplicarFiltroVisitas();

                if (MostrarAgendamentoDoDia)
                    btn_atualizar.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        private void ultimas_visitas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string placaProcurada = Placa.Normalizar(txt_Placa.Text);

            if (placaProcurada.Length == 0)
            {
                MessageBox.Show("Informe a placa para pesquisar.");
                return;
            }

            bool encontrado = false;

            using (var con = new SQLiteConnection(conexao))
            {
                con.Open();
                string sql = @"
                    SELECT ID, CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, EMPRESA
                    FROM Veiculo
                    WHERE " + PlacaNormalizada + @" = @PLACA
                    ORDER BY DataHora DESC
                    LIMIT 1";

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@PLACA", placaProcurada);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            encontrado = true;
                            txt_RG.Text = dr["CPF"].ToString();
                            txt_NOME.Text = dr["NOME"].ToString();
                            txt_cel.Text = dr["CELULAR"].ToString();
                            txt_RG_A.Text = dr["CPFAJUDANTE"].ToString();
                            txt_NOME_A.Text = dr["NOMEAJUDANTE"].ToString();
                            TIPO.Text = dr["TIPOVEICULO"].ToString();
                            txt_OBS.Text = dr["EMPRESA"].ToString().Trim();
                        }
                    }
                }
            }

            if (encontrado)
            {
                MessageBox.Show("Registro encontrado!");
                att_historico.PerformClick();
            }
            else
            {
                MessageBox.Show("Placa não encontrada!");
                LimparCampo();
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
            if (!MostrarAgendamentoDoDia)
                return;

            DateTime hojeInicio = DateTime.Today;
            DateTime hojeFim = DateTime.Today.AddDays(1).AddSeconds(-1);

            string sql = @"
            SELECT USUARIO, NOME, EMPRESA,
            strftime('%d/%m/%Y %H:%M', DATAHORA) AS DATAHORA
            FROM AGENDAMENTO
            WHERE datetime(DATAHORA) BETWEEN datetime($inicio) AND datetime($fim)
            ORDER BY DATAHORA DESC";

            DataTable dt = new DataTable();

            using (var conn = new SQLiteConnection(conexaoagenda))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("$inicio", hojeInicio.ToString("yyyy-MM-dd HH:mm"));
                    cmd.Parameters.AddWithValue("$fim", hojeFim.ToString("yyyy-MM-dd HH:mm"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }

            dtg_agendamento.DataSource = dt;
        }
        private void time_veiculo_Tick(object sender, EventArgs e)
        {
            // btn_visitas ja atualiza a agenda ao final; nao precisa do segundo clique.
            btn_visitas.PerformClick(); // auto-clique
        }

        private void dt_historico_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // Exemplo 4: Corrija o método att_historico_Click:
        private void att_historico_Click(object sender, EventArgs e)
        {
            string placaFiltro = Placa.Normalizar(txt_Placa.Text);
            string rgFiltro = txt_RG.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(placaFiltro) && string.IsNullOrEmpty(rgFiltro))
            {
                MessageBox.Show("Informe placa ou documento para pesquisa.");
                return;
            }

            try
            {
                DataTable dt = new DataTable();

                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();

                    using (var cmd = new SQLiteCommand(con))
                    {
                        if (!string.IsNullOrEmpty(placaFiltro))
                        {
                            cmd.CommandText = @"
                                SELECT
                                strftime('%d/%m/%Y %H:%M', DataHora) AS 'ENTRADA', SAIDA
                                FROM Veiculo
                                WHERE " + PlacaNormalizada + @" = $placa
                                ORDER BY DataHora DESC";

                            cmd.Parameters.AddWithValue("$placa", placaFiltro);
                        }
                        else
                        {
                            cmd.CommandText = @"
                                SELECT
                                strftime('%d/%m/%Y %H:%M', DataHora) AS 'ENTRADA', SAIDA
                                FROM Veiculo
                                WHERE UPPER(CPF) = $CPF
                                ORDER BY DataHora DESC";

                            cmd.Parameters.AddWithValue("$CPF", rgFiltro);
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void Relatorio_data_Click(object sender, EventArgs e)
        {
            using (Frm_relatorio_data f = new Frm_relatorio_data())
            {
                f.ShowDialog();
            }
        }

        private void Relatorio_personalizado_Click(object sender, EventArgs e)
        {
            using (Frm_relatorio_personalizado f = new Frm_relatorio_personalizado())
            {
                f.ShowDialog(this);
            }
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
        }

        // Exemplo 5: Corrija o método btn_rg_Click:
        private void btn_rg_Click(object sender, EventArgs e)
        {
            string placaCPF = txt_RG.Text.Trim().ToUpper();
            bool encontrado = false;

            using (var con = new SQLiteConnection(conexao))
            {
                con.Open();
                string sql = @"
                    SELECT ID ,CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, EMPRESA
                    FROM Veiculo
                    WHERE @CPF = CPF
                    ORDER BY DataHora DESC
                    LIMIT 1";

                using (var cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@CPF", placaCPF);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            encontrado = true;
                            txt_Placa.Text = dr["PLACA"].ToString();
                            txt_RG.Text = dr["CPF"].ToString();
                            txt_NOME.Text = dr["NOME"].ToString();
                            txt_cel.Text = dr["CELULAR"].ToString();
                            txt_RG_A.Text = dr["CPFAJUDANTE"].ToString();
                            txt_NOME_A.Text = dr["NOMEAJUDANTE"].ToString();
                            TIPO.Text = dr["TIPOVEICULO"].ToString();
                            txt_OBS.Text = dr["EMPRESA"].ToString().Trim();
                        }
                    }
                }
            }

            if (encontrado)
            {
                MessageBox.Show("Registro encontrado!");
                att_historico.PerformClick();
            }
            else
            {
                MessageBox.Show("Documento não encontrado!");
                LimparCampo();
            }
        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {
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
            novaAba.SuspendLayout();

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
            Mascaras.AplicarDocumento(txtRg);

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
            novaAba.ResumeLayout(false);

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
                TabPage ultima = Tab_Ajudantes.TabPages[Tab_Ajudantes.TabPages.Count - 1];
                Tab_Ajudantes.TabPages.Remove(ultima);
                ultima.Dispose(); // libera os controles da aba removida
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

                    // Uma transacao unica para toda a migracao: muito mais rapido
                    // que quatro commits e nao deixa o banco pela metade em caso de erro.
                    using (var tran = con.BeginTransaction())
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.Transaction = tran;

                        // 1. Cria tabela nova com ID
                        cmd.CommandText = @"
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
                        EMPRESA      TEXT,
                        USUARIOENTRADA TEXT
                         )";
                        cmd.ExecuteNonQuery();

                        // 2. Copia dados antigos
                        cmd.CommandText = @"
                        INSERT INTO Veiculo_Nova
                        (CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA, USUARIOENTRADA)
                        SELECT CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA, USUARIOENTRADA
                        FROM Veiculo";
                        cmd.ExecuteNonQuery();

                        // 3. Remove tabela antiga
                        cmd.CommandText = "DROP TABLE Veiculo";
                        cmd.ExecuteNonQuery();

                        // 4. Renomeia
                        cmd.CommandText = "ALTER TABLE Veiculo_Nova RENAME TO Veiculo";
                        cmd.ExecuteNonQuery();

                        tran.Commit();
                    }
                }

                MessageBox.Show("Coluna ID criada com sucesso!", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }

        }

        private void OcultarVisitas_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltroVisitas();
        }

        private void AplicarFiltroVisitas()
        {
            DataTable dt = ultimas_visitas.DataSource as DataTable;
            if (dt == null) return;

            dt.DefaultView.RowFilter = OcultarVisitas.Checked
                ? "SAIDA IS NULL OR SAIDA = ''"
                : string.Empty;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
