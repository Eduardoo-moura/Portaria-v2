using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace Portaria
{
    /// <summary>
    /// Placa de veiculo nos dois formatos em uso no Brasil:
    /// antiga (LLL9999) e Mercosul (LLL9L99).
    ///
    /// O sistema trabalha com os 7 caracteres sem separador, que e como esta a
    /// maior parte dos registros do banco.
    /// </summary>
    public static class Placa
    {
        public const int Tamanho = 7;

        /// <summary>Se o caractere serve na posicao informada (0 a 6).</summary>
        public static bool CaractereValido(int posicao, char c)
        {
            switch (posicao)
            {
                case 0:
                case 1:
                case 2:
                    return Letra(c);
                case 3:
                    return Digito(c);
                case 4:
                    // Letra = Mercosul (ABC1D23); numero = placa antiga (ABC1234).
                    return Letra(c) || Digito(c);
                case 5:
                case 6:
                    return Digito(c);
                default:
                    return false;
            }
        }

        /// <summary>Se o texto e uma placa completa ou o inicio de uma placa possivel.</summary>
        public static bool PrefixoValido(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return true;

            if (texto.Length > Tamanho)
                return false;

            for (int i = 0; i < texto.Length; i++)
            {
                if (!CaractereValido(i, char.ToUpperInvariant(texto[i])))
                    return false;
            }

            return true;
        }

        public static bool Completa(string texto)
        {
            return texto != null && texto.Length == Tamanho && PrefixoValido(texto);
        }

        /// <summary>
        /// Aplica a mascara descartando o que nao encaixa. Usado no texto colado
        /// no campo.
        /// </summary>
        public static string Aplicar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            var placa = new StringBuilder(Tamanho);

            foreach (char bruto in texto)
            {
                if (placa.Length == Tamanho)
                    break;

                char c = char.ToUpperInvariant(bruto);

                if (CaractereValido(placa.Length, c))
                    placa.Append(c);
            }

            return placa.ToString();
        }

        /// <summary>
        /// Placa reduzida a letras e numeros, em maiusculas. Serve para comparar
        /// com os registros antigos, gravados com espaco ("GCT 6604") ou hifen.
        /// </summary>
        public static string Normalizar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            var limpa = new StringBuilder(texto.Length);

            foreach (char bruto in texto)
            {
                char c = char.ToUpperInvariant(bruto);

                if (Letra(c) || Digito(c))
                    limpa.Append(c);
            }

            return limpa.ToString();
        }

        private static bool Letra(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        private static bool Digito(char c)
        {
            return c >= '0' && c <= '9';
        }
    }

    /// <summary>
    /// Documento do motorista/ajudante. O campo aceita RG ou CPF; so o CPF tem
    /// digito verificador para conferir.
    /// </summary>
    public static class Documento
    {
        public const int TamanhoCpf = 11;

        public static bool SoDigitos(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return false;

            foreach (char c in texto)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Trata como CPF o documento com 11 digitos e mais nada. Um RG, que tem
        /// menos digitos ou traz letras e pontos, nao entra nessa conta.
        /// </summary>
        public static bool EhCpf(string texto)
        {
            return texto != null && texto.Length == TamanhoCpf && SoDigitos(texto);
        }

        /// <summary>Confere os dois digitos verificadores do CPF (modulo 11).</summary>
        public static bool CpfValido(string cpf)
        {
            if (!EhCpf(cpf))
                return false;

            // 00000000000, 11111111111... fecham na conta, mas nao existem.
            bool todosIguais = true;
            for (int i = 1; i < TamanhoCpf; i++)
            {
                if (cpf[i] != cpf[0])
                {
                    todosIguais = false;
                    break;
                }
            }

            if (todosIguais)
                return false;

            int[] digitos = new int[TamanhoCpf];
            for (int i = 0; i < TamanhoCpf; i++)
                digitos[i] = cpf[i] - '0';

            return digitos[9] == DigitoVerificador(digitos, 9)
                && digitos[10] == DigitoVerificador(digitos, 10);
        }

        /// <summary>
        /// Digito calculado sobre as primeiras <paramref name="quantidade"/>
        /// posicoes, com pesos decrescentes a partir de quantidade + 1.
        /// </summary>
        private static int DigitoVerificador(int[] digitos, int quantidade)
        {
            int soma = 0;
            for (int i = 0; i < quantidade; i++)
                soma += digitos[i] * (quantidade + 1 - i);

            int resto = soma % 11;

            return resto < 2 ? 0 : 11 - resto;
        }
    }

    /// <summary>
    /// Liga a mascara de placa e a conferencia de CPF nos campos da tela.
    /// </summary>
    public static class Mascaras
    {
        private static readonly Color CorInvalido = Color.FromArgb(255, 205, 205);

        private static readonly ToolTip Aviso = new ToolTip { IsBalloon = true };

        // Cor normal de cada campo, para desfazer o destaque de erro.
        private static readonly Dictionary<TextBox, Color> CorNormal = new Dictionary<TextBox, Color>();

        public static void AplicarPlaca(TextBox campo)
        {
            campo.CharacterCasing = CharacterCasing.Upper;
            campo.KeyPress += Placa_KeyPress;
            campo.TextChanged += Placa_TextChanged;
        }

        public static void AplicarDocumento(TextBox campo)
        {
            CorNormal[campo] = campo.BackColor;
            campo.KeyPress += Documento_KeyPress;
            campo.TextChanged += Documento_TextChanged;

            // As abas de ajudante sao criadas e descartadas a cada consulta. Sem
            // soltar a referencia aqui, este dicionario estatico seguraria para
            // sempre cada campo ja descartado — e, com ele, a aba inteira.
            campo.Disposed += Documento_Disposed;
        }

        private static void Documento_Disposed(object sender, EventArgs e)
        {
            TextBox campo = sender as TextBox;

            if (campo != null)
                CorNormal.Remove(campo);
        }

        private static void Placa_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox campo = (TextBox)sender;

            if (char.IsControl(e.KeyChar)) // backspace, delete, colar...
                return;

            if (!Placa.PrefixoValido(TextoResultante(campo, char.ToUpperInvariant(e.KeyChar))))
            {
                e.Handled = true; // caractere recusado
                SystemSounds.Beep.Play();
            }
        }

        /// <summary>
        /// Texto colado tambem passa pela mascara. O que chega de uma consulta
        /// (campo sem foco) fica como esta no banco: os registros antigos usam
        /// espaco na placa e alguns trazem "S/ PLACA".
        /// </summary>
        private static void Placa_TextChanged(object sender, EventArgs e)
        {
            TextBox campo = (TextBox)sender;

            if (!campo.Focused)
                return;

            string mascarado = Placa.Aplicar(campo.Text);

            if (mascarado == campo.Text)
                return;

            int posicao = campo.SelectionStart - (campo.Text.Length - mascarado.Length);

            campo.Text = mascarado;
            campo.SelectionStart = Math.Max(0, Math.Min(posicao, mascarado.Length));
        }

        private static void Documento_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox campo = (TextBox)sender;

            if (char.IsControl(e.KeyChar))
                return;

            string proposto = TextoResultante(campo, char.ToUpperInvariant(e.KeyChar));

            // Com 11 digitos o documento e um CPF: o digito verificador tem de fechar.
            if (Documento.EhCpf(proposto) && !Documento.CpfValido(proposto))
            {
                e.Handled = true; // numero recusado na hora da digitacao
                SystemSounds.Beep.Play();
                Avisar(campo, "CPF INVALIDO", "O dígito verificador não confere. Confira o número.");
            }
        }

        private static void Documento_TextChanged(object sender, EventArgs e)
        {
            TextBox campo = (TextBox)sender;

            bool invalido = Documento.EhCpf(campo.Text) && !Documento.CpfValido(campo.Text);

            campo.BackColor = invalido ? CorInvalido : CorPadrao(campo);
        }

        private static Color CorPadrao(TextBox campo)
        {
            Color cor;
            return CorNormal.TryGetValue(campo, out cor) ? cor : SystemColors.Window;
        }

        private static void Avisar(Control campo, string titulo, string mensagem)
        {
            Aviso.ToolTipTitle = titulo;
            Aviso.Show(mensagem, campo, 0, campo.Height + 2, 2500);
        }

        /// <summary>Como o texto do campo fica se o caractere for aceito.</summary>
        private static string TextoResultante(TextBox campo, char c)
        {
            string texto = campo.Text;
            int inicio = campo.SelectionStart;
            int selecionado = campo.SelectionLength;

            if (inicio > texto.Length)
                inicio = texto.Length;

            if (inicio + selecionado > texto.Length)
                selecionado = texto.Length - inicio;

            return texto.Substring(0, inicio) + c + texto.Substring(inicio + selecionado);
        }
    }
}
