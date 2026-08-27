using System;
using System.Drawing;
using System.Windows.Forms;

namespace Portaria
{
    /// <summary>
    /// Pergunta o nome de quem esta retirando a mercadoria na confirmacao da
    /// entrega. A tela e montada em codigo por ser um dialogo de um campo so.
    /// </summary>
    public class Frm_RetiradaMercadoria : Form
    {
        private readonly TextBox txt_Retirante;

        /// <summary>Nome informado pelo porteiro, ja sem espacos nas pontas.</summary>
        public string NomeRetirante
        {
            get { return txt_Retirante.Text.Trim(); }
        }

        public Frm_RetiradaMercadoria(string mercadoria, string nomeAtual)
        {
            Font fonte = new Font("Arial Narrow", 12F);

            Label lbl_mercadoria = new Label
            {
                AutoSize = false,
                Font = new Font("Arial Narrow", 12F, FontStyle.Bold),
                Location = new Point(12, 12),
                Size = new Size(456, 46),
                Text = mercadoria
            };

            Label lbl_retirante = new Label
            {
                AutoSize = true,
                Font = fonte,
                Location = new Point(12, 70),
                Text = "RETIRADO POR"
            };

            txt_Retirante = new TextBox
            {
                CharacterCasing = CharacterCasing.Upper,
                Font = fonte,
                Location = new Point(140, 66),
                Size = new Size(328, 26),
                Text = nomeAtual ?? string.Empty
            };

            Button btn_ok = new Button
            {
                DialogResult = DialogResult.OK,
                Font = new Font("Arial Narrow", 12F, FontStyle.Bold),
                Location = new Point(228, 106),
                Size = new Size(115, 36),
                Text = "CONFIRMAR",
                UseVisualStyleBackColor = true
            };

            Button btn_cancelar = new Button
            {
                DialogResult = DialogResult.Cancel,
                Font = fonte,
                Location = new Point(353, 106),
                Size = new Size(115, 36),
                Text = "CANCELAR",
                UseVisualStyleBackColor = true
            };

            // Sem nome nao ha registro de quem levou: o dialogo nao fecha no OK.
            btn_ok.Click += (s, e) =>
            {
                if (NomeRetirante.Length > 0)
                    return;

                MessageBox.Show("Informe o nome de quem está retirando a mercadoria!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                DialogResult = DialogResult.None;
                txt_Retirante.Focus();
            };

            AcceptButton = btn_ok;
            CancelButton = btn_cancelar;
            ClientSize = new Size(480, 154);
            Controls.Add(lbl_mercadoria);
            Controls.Add(lbl_retirante);
            Controls.Add(txt_Retirante);
            Controls.Add(btn_ok);
            Controls.Add(btn_cancelar);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Frm_RetiradaMercadoria";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CONFIRMAÇÃO DE ENTREGA";
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            txt_Retirante.Focus();
            txt_Retirante.SelectAll();
        }
    }
}
