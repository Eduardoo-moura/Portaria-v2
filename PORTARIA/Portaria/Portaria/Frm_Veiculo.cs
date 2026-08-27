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

        // A grade das mercadorias na portaria e estreita: fonte menor que a
        // das outras para caber CHEGADA, DESTINATARIO e EMPRESA.
        private static readonly Font FonteMercadoriaCelula = new Font("Segoe UI", 9);
        private static readonly Font FonteMercadoriaCabecalho = new Font("Segoe UI", 9, FontStyle.Bold);

        // Fonte dos campos das abas de ajudante criadas em tempo de execucao.
        // Sem ela os campos herdam o tamanho 24 da TabPage e saem gigantes.
        private static readonly Font FonteCampoAjudante = new Font("Arial Narrow", 12F);

        /// <summary>
        /// Quantos ajudantes uma entrada comporta. O ajudante 1 fica na aba fixa
        /// do formulario; do 2 ao 5 as abas sao criadas pelo botao "+".
        /// </summary>
        private const int MaxAjudantes = 5;

        /// <summary>
        /// Por quantos dias um veiculo sem saida continua aparecendo na grade,
        /// contando o dia de hoje. Sem isso a entrada some na virada do dia e a
        /// saida nunca mais pode ser registrada, porque o botao SAIDA so age
        /// sobre a linha selecionada aqui.
        ///
        /// O limite existe para a grade nao virar um deposito: pendencia parada
        /// ha meses nao e operacao do turno, e dar saida nela hoje gravaria uma
        /// hora de saida falsa.
        /// </summary>
        private const int DiasPendenciasNaGrade = 3;

        // Pendencia de dia anterior fica em ambar na grade, para nao se misturar
        // com o movimento de hoje. A cor de selecao tambem muda: sem isso a linha
        // selecionada volta ao azul padrao e a marcacao some da vista.
        private static readonly Color FundoPendencia = Color.FromArgb(255, 242, 204);
        private static readonly Color TextoPendencia = Color.FromArgb(124, 94, 0);
        private static readonly Color FundoPendenciaSelecionada = Color.FromArgb(191, 143, 0);
        private static readonly Color TextoPendenciaSelecionada = Color.White;

        /// <summary>Coluna tecnica que marca a linha vinda de um dia anterior.</summary>
        private const string ColunaPendencia = "PENDENCIAANTIGA";

        // Troca de turno: item proprio na barra de menu, encostado no nome do
        // usuario logado. Nao entra em RELATORIO nem em USUARIOS — trocar de
        // turno nao e emitir relatorio nem cadastrar gente, e o nivel 2, que e
        // quem mais troca, nem enxerga o menu USUARIOS.
        private readonly ToolStripMenuItem Item_trocar_usuario =
            new ToolStripMenuItem("TROCAR USUARIO");

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

            // Alinhado a direita e inserido antes do rotulo do usuario: entre os
            // itens alinhados a direita, o primeiro da colecao e o que fica mais
            // a direita, entao o rotulo sai na ponta e o botao logo a esquerda —
            // "TROCAR USUARIO   USUARIO: FULANO".
            Item_trocar_usuario.Name = "Item_trocar_usuario";
            Item_trocar_usuario.Alignment = ToolStripItemAlignment.Right;
            Item_trocar_usuario.Click += Item_trocar_usuario_Click;

            menuStrip1.Items.Insert(
                menuStrip1.Items.IndexOf(lbl_usuario_logado) + 1, Item_trocar_usuario);
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

            dtg_mercadorias_pendentes.ReadOnly = true;
            dtg_mercadorias_pendentes.DefaultCellStyle.Font = FonteMercadoriaCelula;
            dtg_mercadorias_pendentes.ColumnHeadersDefaultCellStyle.Font = FonteMercadoriaCabecalho;
            dtg_mercadorias_pendentes.AlternatingRowsDefaultCellStyle = dtg_mercadorias_pendentes.DefaultCellStyle;
            dtg_mercadorias_pendentes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            AtualizarMercadoriasPendentes();
        }

        /// <summary>
        /// Mercadorias que continuam na portaria esperando o destinatario retirar.
        /// So chegada, destinatario e empresa: e um lembrete a vista do porteiro,
        /// o detalhe fica na tela de mercadorias.
        /// </summary>
        private void AtualizarMercadoriasPendentes()
        {
            try
            {
                DataTable dt = new DataTable();

                using (var con = new SQLiteConnection(conexao))
                {
                    con.Open();
                    string sql = @"
                    SELECT strftime('%d/%m/%Y %H:%M', DATAHORA) AS 'CHEGADA',
                    DESTINATARIO AS 'DESTINATARIO',
                    EMPRESA
                    FROM MERCADORIA
                    WHERE IFNULL(ENTREGUE,'') <> 'SIM'
                    ORDER BY DATAHORA DESC, ID DESC";

                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                dtg_mercadorias_pendentes.DataSource = dt;
                lbl_mercadorias_pendentes.Text = "MERCADORIAS NA PORTARIA (" + dt.Rows.Count + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar as mercadorias na portaria: " + ex.Message);
            }
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
                Item_trocar_usuario.Visible = false;
                return;
            }

            lbl_usuario_logado.Font = FonteUsuarioLogado;
            lbl_usuario_logado.Text = "USUARIO: " + usuario.NomeExibicao;
            Strip_usuarios.Visible = usuario.PodeCadastrarUsuario;

            // A troca de turno serve aos dois niveis: fica sempre a vista.
            Item_trocar_usuario.Visible = true;
        }

        private void Item_trocar_usuario_Click(object sender, EventArgs e)
        {
            TrocarUsuario();
        }

        /// <summary>
        /// Troca de turno sem fechar o sistema. Existe porque USUARIOENTRADA sai
        /// de Sessao.Atual no momento em que a entrada e gravada: sem trocar o
        /// usuario, o turno inteiro fica registrado no nome de quem abriu o
        /// programa de manha.
        /// </summary>
        private void TrocarUsuario()
        {
            // O que esta digitado e ainda nao foi salvo seria gravado no nome do
            // porteiro que entra. Melhor descartar de forma explicita do que
            // atribuir a entrada a quem nao atendeu o veiculo.
            if (HaDadosNaTela())
            {
                DialogResult resposta = MessageBox.Show(
                    "Há dados digitados que ainda não foram salvos." + Environment.NewLine +
                    "Trocar de usuário agora descarta esses dados. Continuar?",
                    "Troca de turno", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (resposta != DialogResult.Yes)
                    return;
            }

            UsuarioInfo anterior = Sessao.Atual;

            using (var login = new Frm_Login())
            {
                login.Text = "TROCA DE TURNO";
                login.StartPosition = FormStartPosition.CenterParent;

                // Cancelar mantem quem ja estava: o sistema nunca fica aberto
                // sem usuario na sessao.
                if (login.ShowDialog(this) != DialogResult.OK)
                    return;
            }

            LimparCampo();

            // Refaz o menu e o nome na barra: o porteiro que assume pode ter
            // nivel diferente do que saiu.
            AplicarUsuarioLogado();

            // Quem assume o turno comeca com a grade e as pendencias atualizadas.
            btn_visitas.PerformClick();

            string aviso = "Turno de " + Sessao.Atual.NomeExibicao + " iniciado.";

            if (anterior != null)
                aviso += Environment.NewLine + "Turno anterior: " + anterior.NomeExibicao + ".";

            MessageBox.Show(aviso, "Troca de turno",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Se ha entrada em digitacao na tela. Usado antes da troca de turno,
        /// para nao descartar em silencio o que o porteiro anterior deixou pela
        /// metade.
        /// </summary>
        private bool HaDadosNaTela()
        {
            return txt_Placa.Text.Trim().Length > 0
                || txt_RG.Text.Trim().Length > 0
                || txt_NOME.Text.Trim().Length > 0
                || txt_cel.Text.Trim().Length > 0
                || txt_OBS.Text.Trim().Length > 0
                || TIPO.Text.Trim().Length > 0
                || AjudantesPreenchidos().Count > 0;
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

        /// <summary>
        /// Abre o registro das mercadorias que chegam na portaria. A tela e modal:
        /// enquanto ela estiver aberta a entrada de veiculos fica parada, que e o
        /// comportamento das demais telas do sistema.
        /// </summary>
        private void Btn_Mercadorias_Click(object sender, EventArgs e)
        {
            using (Frm_Mercadoria f = new Frm_Mercadoria())
            {
                f.ShowDialog(this);
            }

            // O que foi registrado ou entregue la dentro muda a lista daqui.
            AtualizarMercadoriasPendentes();
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

            // As abas do 2 ao 5 saem de cena: sobra a aba fixa, ja vazia, como
            // no estado em que a tela abre.
            RemoverAbasExtras();
        }

        // ----- ajudantes -----
        //
        // Cada ajudante ocupa um "lugar" de 1 a MaxAjudantes. O lugar 1 e a aba
        // fixa do formulario (txt_RG_A / txt_NOME_A) e grava em CPFAJUDANTE /
        // NOMEAJUDANTE; do 2 ao 5 as abas sao criadas em tempo de execucao e
        // gravam nas colunas numeradas. Os nomes de controle e de coluna sao
        // derivados do numero do lugar, para tela e banco nao saírem de sincronia.

        private static string NomeCampoRg(int lugar)
        {
            return lugar == 1 ? "txt_RG_A" : "txtRgAjudante" + lugar;
        }

        private static string NomeCampoNome(int lugar)
        {
            return lugar == 1 ? "txt_NOME_A" : "txtNomeAjudante" + lugar;
        }

        private static string ColunaRg(int lugar)
        {
            return lugar == 1 ? "CPFAJUDANTE" : "CPFAJUDANTE" + lugar;
        }

        private static string ColunaNome(int lugar)
        {
            return lugar == 1 ? "NOMEAJUDANTE" : "NOMEAJUDANTE" + lugar;
        }

        /// <summary>Campo de um lugar de ajudante, ou null quando a aba nao existe.</summary>
        private TextBox CampoAjudante(string nomeDoControle)
        {
            foreach (TabPage aba in Tab_Ajudantes.TabPages)
            {
                Control[] achados = aba.Controls.Find(nomeDoControle, false);

                if (achados.Length > 0)
                    return achados[0] as TextBox;
            }

            return null;
        }

        /// <summary>Aba de um lugar de ajudante, ou null quando ainda nao foi criada.</summary>
        private TabPage AbaDoAjudante(int lugar)
        {
            string nome = NomeCampoRg(lugar);

            foreach (TabPage aba in Tab_Ajudantes.TabPages)
            {
                if (aba.Controls.Find(nome, false).Length > 0)
                    return aba;
            }

            return null;
        }

        private static string TextoDe(TextBox campo)
        {
            return campo == null ? "" : campo.Text.Trim();
        }

        /// <summary>
        /// Os ajudantes preenchidos na tela, na ordem das abas e sem buracos:
        /// quem deixou a aba 2 em branco e digitou na 3 grava como ajudante 2.
        /// Assim as colunas do banco sao sempre preenchidas em sequencia.
        /// </summary>
        private List<string[]> AjudantesPreenchidos()
        {
            var preenchidos = new List<string[]>();

            for (int lugar = 1; lugar <= MaxAjudantes; lugar++)
            {
                string documento = TextoDe(CampoAjudante(NomeCampoRg(lugar)));
                string nome = TextoDe(CampoAjudante(NomeCampoNome(lugar)));

                if (documento.Length == 0 && nome.Length == 0)
                    continue;

                preenchidos.Add(new string[] { documento, nome });
            }

            return preenchidos;
        }

        /// <summary>
        /// Traz para a tela os ajudantes da visita encontrada: cria as abas que
        /// faltarem e descarta as que sobrarem, para o formulario mostrar
        /// exatamente o que esta gravado — nem mais, nem menos.
        /// </summary>
        private void CarregarAjudantes(SQLiteDataReader dr)
        {
            RemoverAbasExtras();

            for (int lugar = 1; lugar <= MaxAjudantes; lugar++)
            {
                string documento = ValorDaColuna(dr, ColunaRg(lugar));
                string nome = ValorDaColuna(dr, ColunaNome(lugar));

                if (documento.Length == 0 && nome.Length == 0)
                    continue;

                GarantirAbaAjudante(lugar);

                TextBox campoRg = CampoAjudante(NomeCampoRg(lugar));
                TextBox campoNome = CampoAjudante(NomeCampoNome(lugar));

                if (campoRg != null)
                    campoRg.Text = documento;

                if (campoNome != null)
                    campoNome.Text = nome;
            }

            if (Tab_Ajudantes.TabPages.Count > 0)
                Tab_Ajudantes.SelectedIndex = 0;
        }

        private static string ValorDaColuna(SQLiteDataReader dr, string coluna)
        {
            object valor = dr[coluna];

            return valor == null || valor == DBNull.Value ? "" : valor.ToString().Trim();
        }

        /// <summary>Devolve a aba do lugar informado, criando-a se ainda nao existir.</summary>
        private TabPage GarantirAbaAjudante(int lugar)
        {
            return AbaDoAjudante(lugar) ?? CriarAbaAjudante(lugar);
        }

        /// <summary>
        /// Monta uma aba de ajudante com a mesma geometria e a mesma fonte da aba
        /// fixa, para as abas novas nao destoarem nem estourarem a largura.
        /// </summary>
        private TabPage CriarAbaAjudante(int lugar)
        {
            TabPage novaAba = new TabPage("AJUDANTE " + lugar);
            novaAba.UseVisualStyleBackColor = true;
            novaAba.SuspendLayout();

            Label lblRg = new Label
            {
                AutoSize = true,
                Font = FonteCampoAjudante,
                Location = new Point(3, 10),
                Text = "RG / CPF"
            };

            TextBox txtRg = new TextBox
            {
                CharacterCasing = CharacterCasing.Upper,
                Font = FonteCampoAjudante,
                Location = new Point(78, 4),
                Name = NomeCampoRg(lugar),
                Size = new Size(102, 26)
            };

            Mascaras.AplicarDocumento(txtRg);

            Label lblNome = new Label
            {
                AutoSize = true,
                Font = FonteCampoAjudante,
                Location = new Point(6, 42),
                Text = "NOME"
            };

            TextBox txtNome = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                CharacterCasing = CharacterCasing.Upper,
                Font = FonteCampoAjudante,
                Location = new Point(77, 39),
                Name = NomeCampoNome(lugar),
                Size = new Size(316, 26)
            };

            novaAba.Controls.Add(lblRg);
            novaAba.Controls.Add(txtRg);
            novaAba.Controls.Add(lblNome);
            novaAba.Controls.Add(txtNome);
            novaAba.ResumeLayout(false);

            Tab_Ajudantes.TabPages.Add(novaAba);

            return novaAba;
        }

        /// <summary>Fecha as abas criadas em tempo de execucao e limpa a aba fixa.</summary>
        private void RemoverAbasExtras()
        {
            for (int i = Tab_Ajudantes.TabPages.Count - 1; i >= 0; i--)
            {
                TabPage aba = Tab_Ajudantes.TabPages[i];

                if (aba == Tab_Ajudante1)
                    continue;

                Tab_Ajudantes.TabPages.Remove(aba);
                aba.Dispose();
            }

            txt_RG_A.Clear();
            txt_NOME_A.Clear();

            if (Tab_Ajudantes.TabPages.Count > 0)
                Tab_Ajudantes.SelectedIndex = 0;
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

            // Os ajudantes digitados nas abas, ja compactados na ordem em que
            // serao gravados nos lugares 1..MaxAjudantes.
            List<string[]> ajudantes = AjudantesPreenchidos();

            using (SQLiteConnection conn = new SQLiteConnection(conexao))
            {
                conn.Open();
                string sql = @"
                INSERT INTO Veiculo
                (CPF, NOME, CELULAR,
                 CPFAJUDANTE,  NOMEAJUDANTE,
                 CPFAJUDANTE2, NOMEAJUDANTE2,
                 CPFAJUDANTE3, NOMEAJUDANTE3,
                 CPFAJUDANTE4, NOMEAJUDANTE4,
                 CPFAJUDANTE5, NOMEAJUDANTE5,
                 DataHora, SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA, USUARIOENTRADA)
                VALUES
                (@CPF, @NOME, @CELULAR,
                 @CPFAJUDANTE1, @NOMEAJUDANTE1,
                 @CPFAJUDANTE2, @NOMEAJUDANTE2,
                 @CPFAJUDANTE3, @NOMEAJUDANTE3,
                 @CPFAJUDANTE4, @NOMEAJUDANTE4,
                 @CPFAJUDANTE5, @NOMEAJUDANTE5,
                 @DataHora, @SAIDA, @PLACA, @TIPOVEICULO, @PRESTADOR, @AGREGADO, @EMPRESA, @USUARIOENTRADA)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@CPF", txt_RG.Text);
                    cmd.Parameters.AddWithValue("@NOME", txt_NOME.Text);
                    cmd.Parameters.AddWithValue("@CELULAR", txt_cel.Text);

                    // Lugares sem ajudante vao em branco, e nao nulos: e o mesmo
                    // que o sistema sempre gravou quando nao havia ajudante.
                    for (int lugar = 1; lugar <= MaxAjudantes; lugar++)
                    {
                        string[] ajudante = lugar <= ajudantes.Count
                            ? ajudantes[lugar - 1]
                            : new string[] { "", "" };

                        cmd.Parameters.AddWithValue("@CPFAJUDANTE" + lugar, ajudante[0]);
                        cmd.Parameters.AddWithValue("@NOMEAJUDANTE" + lugar, ajudante[1]);
                    }

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

                    // A grade tem duas origens: tudo o que entrou hoje, e o que
                    // continua sem saida nos ultimos DiasPendenciasNaGrade dias.
                    // A segunda parte e o que faz o veiculo que pernoita seguir
                    // alcancavel pelo botao SAIDA depois da virada do turno.
                    //
                    // O dia e comparado em hora local. DataHora e gravado com
                    // DateTime.Now, mas DATE('now') puro devolve a data UTC: no
                    // horario de Brasilia a data virava as 21h e a grade ficava
                    // vazia pelo resto da noite.
                    //
                    // O teste de DataHora preenchida deixa explicito que as 11 mil
                    // linhas da importacao antiga — sem data e sem saida — ficam
                    // de fora. Hoje elas ja cairiam na comparacao de data, porque
                    // DATE('') e NULL, mas depender disso e fragil: basta alguem
                    // trocar o >= por uma comparacao de texto para as 11 mil
                    // entrarem na grade de uma vez.
                    string sql = @"
                    SELECT ID, CPF, NOME, CELULAR,
                    CPFAJUDANTE AS 'CPF AJUDANTE',
                    NOMEAJUDANTE AS 'NOME AJUDANTE',
                    strftime('%d/%m/%Y %H:%M', DataHora) AS 'ENTRADA',
                    SAIDA, PLACA, TIPOVEICULO AS 'TIPO VEICULO',
                    PRESTADOR, AGREGADO, EMPRESA,
                    CASE WHEN DATE(DataHora) < DATE('now', 'localtime')
                         THEN 'SIM' ELSE '' END AS " + ColunaPendencia + @"
                    FROM Veiculo
                    WHERE TRIM(IFNULL(DataHora,'')) <> ''
                      AND (
                            DATE(DataHora) = DATE('now', 'localtime')
                            OR (
                                 TRIM(IFNULL(SAIDA,'')) = ''
                                 AND DATE(DataHora) >= DATE('now', 'localtime', @DIAS)
                               )
                          )
                    ORDER BY DataHora DESC";

                    using (var cmd = new SQLiteCommand(sql, con))
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        // -2 dias mais o de hoje fecham os 3 dias de pendencia.
                        cmd.Parameters.AddWithValue("@DIAS",
                            "-" + (DiasPendenciasNaGrade - 1) + " days");

                        da.Fill(dt);
                    }
                }

                ultimas_visitas.DataSource = dt;

                // Oculta ID somente se a coluna existir
                if (ultimas_visitas.Columns["ID"] != null)
                    ultimas_visitas.Columns["ID"].Visible = false;

                // Serve so para pintar a linha; a data ja aparece em ENTRADA.
                if (ultimas_visitas.Columns[ColunaPendencia] != null)
                    ultimas_visitas.Columns[ColunaPendencia].Visible = false;

                // Comportamento preservado: o filtro volta marcado a cada atualizacao.
                OcultarVisitas.Checked = true;
                AplicarFiltroVisitas();

                if (MostrarAgendamentoDoDia)
                    btn_atualizar.PerformClick();

                AtualizarMercadoriasPendentes();
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
                    SELECT ID, CPF, NOME, CELULAR,
                    CPFAJUDANTE,  NOMEAJUDANTE,
                    CPFAJUDANTE2, NOMEAJUDANTE2,
                    CPFAJUDANTE3, NOMEAJUDANTE3,
                    CPFAJUDANTE4, NOMEAJUDANTE4,
                    CPFAJUDANTE5, NOMEAJUDANTE5,
                    DataHora, PLACA, TIPOVEICULO, EMPRESA
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
                            CarregarAjudantes(dr);
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
                    SELECT ID, CPF, NOME, CELULAR,
                    CPFAJUDANTE,  NOMEAJUDANTE,
                    CPFAJUDANTE2, NOMEAJUDANTE2,
                    CPFAJUDANTE3, NOMEAJUDANTE3,
                    CPFAJUDANTE4, NOMEAJUDANTE4,
                    CPFAJUDANTE5, NOMEAJUDANTE5,
                    DataHora, PLACA, TIPOVEICULO, EMPRESA
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
                            CarregarAjudantes(dr);
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
        private void Btn_AbaAjudante_Click(object sender, EventArgs e)
        {
            int lugar = ProximoLugarLivre();

            if (lugar == 0)
            {
                MessageBox.Show(
                    "Cada entrada registra no máximo " + MaxAjudantes + " ajudantes.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Tab_Ajudantes.SelectedTab = CriarAbaAjudante(lugar);
        }

        /// <summary>
        /// Menor lugar de ajudante ainda sem aba, ou 0 quando os
        /// <see cref="MaxAjudantes"/> lugares ja estao na tela.
        /// </summary>
        private int ProximoLugarLivre()
        {
            for (int lugar = 1; lugar <= MaxAjudantes; lugar++)
            {
                if (AbaDoAjudante(lugar) == null)
                    return lugar;
            }

            return 0;
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

            // A grade agora mistura o movimento de hoje com pendencias de dias
            // anteriores. Numa pendencia a pergunta diz de quando e o veiculo,
            // para a saida de hoje nao ser dada na linha errada.
            var confirma = MessageBox.Show(EhPendenciaAntiga(ultimas_visitas.SelectedRows[0])
                    ? "Este veículo entrou em " + EntradaDaLinha(ultimas_visitas.SelectedRows[0])
                      + " e continua sem saída." + Environment.NewLine
                      + "Registrar a saída agora?"
                    : "Confirmar saída?",
                "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

            // Trocar o filtro refaz as linhas da grade, entao a pintura vem
            // depois — e nao junto do DataSource, onde seria descartada.
            PintarPendencias();
        }

        /// <summary>Se a linha da grade e uma pendencia vinda de um dia anterior.</summary>
        private static bool EhPendenciaAntiga(DataGridViewRow linha)
        {
            DataGridViewCell celula = linha.Cells[ColunaPendencia];

            return celula != null && Convert.ToString(celula.Value) == "SIM";
        }

        /// <summary>Data e hora de entrada da linha, como ja aparece na grade.</summary>
        private static string EntradaDaLinha(DataGridViewRow linha)
        {
            DataGridViewCell celula = linha.Cells["ENTRADA"];

            return celula == null ? "" : Convert.ToString(celula.Value);
        }

        /// <summary>
        /// Deixa em ambar as linhas que entraram em dias anteriores e continuam
        /// sem saida. Sem isso elas se misturariam ao movimento de hoje, e o
        /// porteiro daria saida achando que era uma entrada recente.
        /// </summary>
        private void PintarPendencias()
        {
            foreach (DataGridViewRow linha in ultimas_visitas.Rows)
            {
                if (!EhPendenciaAntiga(linha))
                    continue;

                linha.DefaultCellStyle.BackColor = FundoPendencia;
                linha.DefaultCellStyle.ForeColor = TextoPendencia;
                linha.DefaultCellStyle.SelectionBackColor = FundoPendenciaSelecionada;
                linha.DefaultCellStyle.SelectionForeColor = TextoPendenciaSelecionada;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
