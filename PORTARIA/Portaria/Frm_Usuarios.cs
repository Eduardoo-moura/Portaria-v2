using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace Portaria
{
    /// <summary>
    /// Cadastro de usuarios. Somente nivel 1 (acesso total) chega aqui.
    /// </summary>
    public partial class Frm_Usuarios : Form
    {
        private const int TamanhoMinimoSenha = 4;

        private static readonly Font FonteCelula = new Font("Segoe UI", 12);
        private static readonly Font FonteCabecalhoColuna = new Font("Segoe UI", 12, FontStyle.Bold);

        private const string TituloInclusao = "NOVO USUARIO";
        private const string TextoBotaoInclusao = "SALVAR";
        private const string TextoBotaoAlteracao = "SALVAR ALTERAÇÃO";

        /// <summary>Id do usuario carregado por duplo clique; -1 quando esta incluindo.</summary>
        private long idEmEdicao = -1;

        public Frm_Usuarios()
        {
            InitializeComponent();
        }

        private void Frm_Usuarios_Load(object sender, EventArgs e)
        {
            cmb_nivel.Items.Add(Nivel.Descricao(Nivel.Total));
            cmb_nivel.Items.Add(Nivel.Descricao(Nivel.Restrito));
            cmb_nivel.SelectedIndex = 1; // por padrao, o nivel mais restrito

            dtg_usuarios.DefaultCellStyle.Font = FonteCelula;
            dtg_usuarios.ColumnHeadersDefaultCellStyle.Font = FonteCabecalhoColuna;

            Carregar();
        }

        private int NivelSelecionado
        {
            get { return cmb_nivel.SelectedIndex == 0 ? Nivel.Total : Nivel.Restrito; }
        }

        private void Carregar()
        {
            try
            {
                dtg_usuarios.DataSource = Usuarios.Listar();

                if (dtg_usuarios.Columns["ID"] != null)
                    dtg_usuarios.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao listar usuários: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Id do usuario selecionado na lista; -1 quando nao ha selecao.</summary>
        private long IdSelecionado()
        {
            if (dtg_usuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um usuário na lista!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return -1;
            }

            object valor = dtg_usuarios.SelectedRows[0].Cells["ID"].Value;
            if (valor == null || valor == DBNull.Value)
                return -1;

            return Convert.ToInt64(valor);
        }

        private string LoginSelecionado()
        {
            object valor = dtg_usuarios.SelectedRows[0].Cells["LOGIN"].Value;
            return valor == null ? "" : valor.ToString();
        }

        /// <summary>
        /// Duplo clique na lista: traz os dados do usuario para os campos.
        /// A senha nao vem (fica guardada criptografada) e so e trocada se for digitada.
        /// </summary>
        private void dtg_usuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow linha = dtg_usuarios.Rows[e.RowIndex];

            object valorId = linha.Cells["ID"].Value;
            if (valorId == null || valorId == DBNull.Value) return;

            idEmEdicao = Convert.ToInt64(valorId);

            txt_login.Text = Texto(linha.Cells["LOGIN"].Value);
            txt_nome.Text = Texto(linha.Cells["NOME"].Value);
            txt_senha.Clear();
            txt_confirma.Clear();

            object valorNivel = linha.Cells["NIVEL"].Value;
            int nivel = valorNivel == null || valorNivel == DBNull.Value
                ? Nivel.Restrito
                : Convert.ToInt32(valorNivel);
            cmb_nivel.SelectedIndex = nivel == Nivel.Total ? 0 : 1;

            grp_novo.Text = string.Format("ALTERANDO {0}  —  DEIXE A SENHA EM BRANCO PARA MANTER A ATUAL",
                txt_login.Text);
            btn_salvar.Text = TextoBotaoAlteracao;

            txt_nome.Focus();
            txt_nome.SelectAll();
        }

        private static string Texto(object valor)
        {
            return valor == null || valor == DBNull.Value ? "" : valor.ToString();
        }

        private void SairModoEdicao()
        {
            idEmEdicao = -1;
            grp_novo.Text = TituloInclusao;
            btn_salvar.Text = TextoBotaoInclusao;
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            if (idEmEdicao >= 0)
            {
                SalvarAlteracao();
                return;
            }

            string login = txt_login.Text.Trim();
            string nome = txt_nome.Text.Trim();
            string senha = txt_senha.Text;
            string confirma = txt_confirma.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("O campo USUARIO é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_login.Focus();
                return;
            }

            if (!SenhaValida(senha, confirma))
                return;

            try
            {
                if (Usuarios.Existe(login))
                {
                    MessageBox.Show("Já existe um usuário com esse nome!", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_login.Focus();
                    return;
                }

                Usuarios.Criar(login, nome, senha, NivelSelecionado);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Não foi possível cadastrar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Usuário cadastrado!", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            Limpar();
            Carregar();
        }

        /// <summary>Grava as alteracoes do usuario trazido por duplo clique.</summary>
        private void SalvarAlteracao()
        {
            string login = txt_login.Text.Trim();
            string nome = txt_nome.Text.Trim();
            string senha = txt_senha.Text;
            string confirma = txt_confirma.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("O campo USUARIO é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_login.Focus();
                return;
            }

            // Senha em branco = mantem a atual.
            bool trocarSenha = senha.Length > 0 || confirma.Length > 0;
            if (trocarSenha && !SenhaValida(senha, confirma))
                return;

            int novoNivel = NivelSelecionado;
            bool souEu = Sessao.Atual != null && Sessao.Atual.Id == idEmEdicao;

            try
            {
                if (Usuarios.Existe(login, idEmEdicao))
                {
                    MessageBox.Show("Já existe outro usuário com esse nome!", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_login.Focus();
                    return;
                }

                // Nao deixa o sistema ficar sem ninguem que possa cadastrar usuarios.
                if (novoNivel != Nivel.Total && Usuarios.OutrosAdministradoresAtivos(idEmEdicao) == 0)
                {
                    MessageBox.Show("Este é o único usuário com acesso total ativo. Crie outro antes de rebaixá-lo.",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Usuarios.Atualizar(idEmEdicao, login, nome, novoNivel);

                if (trocarSenha)
                    Usuarios.AlterarSenha(idEmEdicao, senha);
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Não foi possível alterar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(trocarSenha ? "Usuário e senha alterados!" : "Usuário alterado!", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (souEu && novoNivel != Sessao.Atual.Nivel)
            {
                MessageBox.Show("Você alterou o seu próprio nível de acesso. Saia e entre novamente para valer.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Limpar();
            Carregar();
        }

        private bool SenhaValida(string senha, string confirma)
        {
            if (senha.Length < TamanhoMinimoSenha)
            {
                MessageBox.Show($"A senha deve ter pelo menos {TamanhoMinimoSenha} caracteres!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_senha.Focus();
                return false;
            }

            if (senha != confirma)
            {
                MessageBox.Show("A confirmação não confere com a senha!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_confirma.Focus();
                return false;
            }

            return true;
        }

        private void Limpar()
        {
            txt_login.Clear();
            txt_nome.Clear();
            txt_senha.Clear();
            txt_confirma.Clear();
            cmb_nivel.SelectedIndex = 1;
            SairModoEdicao();
        }

        private void btn_limpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btn_senha_Click(object sender, EventArgs e)
        {
            long id = IdSelecionado();
            if (id < 0) return;

            string senha = txt_senha.Text;
            string confirma = txt_confirma.Text;

            if (senha.Length == 0)
            {
                MessageBox.Show("Digite a nova senha nos campos SENHA e CONFIRMAR SENHA e clique novamente.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_senha.Focus();
                return;
            }

            if (!SenhaValida(senha, confirma))
                return;

            string login = LoginSelecionado();

            var confirmacao = MessageBox.Show($"Alterar a senha do usuário {login}?", "Confirmação",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            try
            {
                Usuarios.AlterarSenha(id, senha);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Senha alterada!", "OK",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            txt_senha.Clear();
            txt_confirma.Clear();
        }

        private void btn_nivel_Click(object sender, EventArgs e)
        {
            long id = IdSelecionado();
            if (id < 0) return;

            int novoNivel = NivelSelecionado;
            string login = LoginSelecionado();

            try
            {
                // Nao deixa o sistema ficar sem ninguem que possa cadastrar usuarios.
                if (novoNivel != Nivel.Total && Usuarios.OutrosAdministradoresAtivos(id) == 0)
                {
                    MessageBox.Show("Este é o único usuário com acesso total ativo. Crie outro antes de rebaixá-lo.",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmacao = MessageBox.Show(
                    $"Alterar o nível do usuário {login} para {Nivel.Descricao(novoNivel)}?",
                    "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacao != DialogResult.Yes) return;

                Usuarios.AlterarNivel(id, novoNivel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Carregar();

            if (Sessao.Atual != null && Sessao.Atual.Id == id)
            {
                MessageBox.Show("Você alterou o seu próprio nível. Saia e entre novamente para valer.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_ativo_Click(object sender, EventArgs e)
        {
            long id = IdSelecionado();
            if (id < 0) return;

            if (Sessao.Atual != null && Sessao.Atual.Id == id)
            {
                MessageBox.Show("Não é possível desativar o usuário que está logado!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string login = LoginSelecionado();
            object valorAtivo = dtg_usuarios.SelectedRows[0].Cells["ATIVO"].Value;
            bool estaAtivo = valorAtivo != null && valorAtivo.ToString() == "SIM";
            bool novoEstado = !estaAtivo;

            try
            {
                if (!novoEstado && Usuarios.OutrosAdministradoresAtivos(id) == 0)
                {
                    MessageBox.Show("Este é o único usuário com acesso total ativo. Crie outro antes de desativá-lo.",
                        "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirmacao = MessageBox.Show(
                    $"{(novoEstado ? "Ativar" : "Desativar")} o usuário {login}?",
                    "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacao != DialogResult.Yes) return;

                Usuarios.DefinirAtivo(id, novoEstado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Carregar();
        }
    }
}
