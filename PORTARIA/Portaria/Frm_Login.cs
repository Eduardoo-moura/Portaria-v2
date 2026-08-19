using System;
using System.Windows.Forms;

namespace Portaria
{
    public partial class Frm_Login : Form
    {
        /// <summary>Usuario autenticado; preenchido apenas quando o login da certo.</summary>
        public UsuarioInfo Usuario { get; private set; }

        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btn_entrar_Click(object sender, EventArgs e)
        {
            string login = txt_usuario.Text.Trim();
            string senha = txt_senha.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Informe o usuário!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_usuario.Focus();
                return;
            }

            if (senha.Length == 0)
            {
                MessageBox.Show("Informe a senha!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_senha.Focus();
                return;
            }

            UsuarioInfo usuario;

            Cursor anterior = this.Cursor;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                usuario = Usuarios.Autenticar(login, senha);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao validar o acesso: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.Cursor = anterior;
            }

            if (usuario == null)
            {
                // Mensagem unica: nao revela se o erro foi no usuario ou na senha.
                MessageBox.Show("Usuário ou senha inválidos!", "Acesso negado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_senha.Clear();
                txt_senha.Focus();
                return;
            }

            Usuario = usuario;
            Sessao.Atual = usuario;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
