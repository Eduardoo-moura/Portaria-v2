using System;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace Portaria
{
    /// <summary>
    /// Niveis de acesso do sistema.
    /// </summary>
    public static class Nivel
    {
        public const int Total = 1;      // acesso geral, inclusive cadastro de usuarios
        public const int Restrito = 2;   // tudo, exceto cadastro de usuarios

        public static string Descricao(int nivel)
        {
            return nivel == Total ? "1 - ACESSO TOTAL" : "2 - SEM CADASTRO DE USUARIO";
        }
    }

    /// <summary>
    /// Dados do usuario autenticado.
    /// </summary>
    public class UsuarioInfo
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Nome { get; set; }
        public int Nivel { get; set; }

        /// <summary>Nome para exibir na tela; cai para o login quando nao houver nome.</summary>
        public string NomeExibicao
        {
            get { return string.IsNullOrWhiteSpace(Nome) ? Login : Nome; }
        }

        public bool PodeCadastrarUsuario
        {
            get { return Nivel == Portaria.Nivel.Total; }
        }
    }

    /// <summary>
    /// Usuario logado na sessao atual.
    /// </summary>
    public static class Sessao
    {
        public static UsuarioInfo Atual { get; set; }
    }

    /// <summary>
    /// Estrutura, autenticacao e cadastro de usuarios.
    /// </summary>
    public static class Usuarios
    {
        // Mesma origem de dados usada pelo restante do sistema.
        private const string Conexao = @"Data Source=ControleAcesso.db;";

        private const int Iteracoes = 50000;
        private const int TamanhoSalt = 16;
        private const int TamanhoHash = 32;

        /// <summary>
        /// Cria a tabela de usuarios (se ainda nao existir) e garante o usuario
        /// admin inicial. Chamado uma vez na abertura do sistema.
        /// </summary>
        public static void GarantirEstrutura()
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS USUARIO (
                        ID     INTEGER PRIMARY KEY AUTOINCREMENT,
                        LOGIN  TEXT NOT NULL COLLATE NOCASE UNIQUE,
                        NOME   TEXT,
                        SENHA  TEXT NOT NULL,
                        NIVEL  INTEGER NOT NULL DEFAULT 2,
                        ATIVO  INTEGER NOT NULL DEFAULT 1
                    )";
                    cmd.ExecuteNonQuery();

                    // Usuario inicial: admin / admin (nivel 1).
                    cmd.CommandText = "SELECT COUNT(*) FROM USUARIO";
                    long total = Convert.ToInt64(cmd.ExecuteScalar());

                    if (total == 0)
                    {
                        cmd.CommandText = @"
                        INSERT INTO USUARIO (LOGIN, NOME, SENHA, NIVEL, ATIVO)
                        VALUES ('admin', 'ADMINISTRADOR', @SENHA, 1, 1)";
                        cmd.Parameters.AddWithValue("@SENHA", GerarHash("admin"));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Devolve o usuario quando login e senha conferem; caso contrario, null.
        /// </summary>
        public static UsuarioInfo Autenticar(string login, string senha)
        {
            if (string.IsNullOrWhiteSpace(login))
                return null;

            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT ID, LOGIN, NOME, SENHA, NIVEL
                    FROM USUARIO
                    WHERE LOGIN = @LOGIN AND ATIVO = 1
                    LIMIT 1";
                    cmd.Parameters.AddWithValue("@LOGIN", login.Trim());

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                            return null;

                        if (!SenhaConfere(senha, dr["SENHA"].ToString()))
                            return null;

                        return new UsuarioInfo
                        {
                            Id = Convert.ToInt64(dr["ID"]),
                            Login = dr["LOGIN"].ToString(),
                            Nome = dr["NOME"] == DBNull.Value ? "" : dr["NOME"].ToString(),
                            Nivel = Convert.ToInt32(dr["NIVEL"])
                        };
                    }
                }
            }
        }

        public static DataTable Listar()
        {
            DataTable dt = new DataTable();

            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                string sql = @"
                SELECT ID, LOGIN, NOME, NIVEL,
                CASE WHEN ATIVO = 1 THEN 'SIM' ELSE 'NAO' END AS ATIVO
                FROM USUARIO
                ORDER BY LOGIN";

                using (var cmd = new SQLiteCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static bool Existe(string login)
        {
            return Existe(login, -1);
        }

        /// <summary>
        /// Se o login ja pertence a alguem. <paramref name="idIgnorar"/> permite
        /// desconsiderar o proprio usuario que esta sendo alterado.
        /// </summary>
        public static bool Existe(string login, long idIgnorar)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM USUARIO WHERE LOGIN = @LOGIN AND ID <> @ID";
                    cmd.Parameters.AddWithValue("@LOGIN", (login ?? "").Trim());
                    cmd.Parameters.AddWithValue("@ID", idIgnorar);
                    return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        /// <summary>Altera login, nome e nivel. A senha nao e tocada aqui.</summary>
        public static void Atualizar(long id, string login, string nome, int nivel)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
                    UPDATE USUARIO
                    SET LOGIN = @LOGIN, NOME = @NOME, NIVEL = @NIVEL
                    WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@LOGIN", login.Trim());
                    cmd.Parameters.AddWithValue("@NOME", (nome ?? "").Trim());
                    cmd.Parameters.AddWithValue("@NIVEL", nivel);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Criar(string login, string nome, string senha, int nivel)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO USUARIO (LOGIN, NOME, SENHA, NIVEL, ATIVO)
                    VALUES (@LOGIN, @NOME, @SENHA, @NIVEL, 1)";
                    cmd.Parameters.AddWithValue("@LOGIN", login.Trim());
                    cmd.Parameters.AddWithValue("@NOME", (nome ?? "").Trim());
                    cmd.Parameters.AddWithValue("@SENHA", GerarHash(senha));
                    cmd.Parameters.AddWithValue("@NIVEL", nivel);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AlterarSenha(long id, string novaSenha)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE USUARIO SET SENHA = @SENHA WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@SENHA", GerarHash(novaSenha));
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AlterarNivel(long id, int nivel)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE USUARIO SET NIVEL = @NIVEL WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@NIVEL", nivel);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DefinirAtivo(long id, bool ativo)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE USUARIO SET ATIVO = @ATIVO WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@ATIVO", ativo ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Quantos usuarios de acesso total ativos existem, sem contar o informado.
        /// Evita que o sistema fique sem ninguem para cadastrar usuarios.
        /// </summary>
        public static long OutrosAdministradoresAtivos(long idIgnorar)
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT COUNT(*) FROM USUARIO
                    WHERE NIVEL = 1 AND ATIVO = 1 AND ID <> @ID";
                    cmd.Parameters.AddWithValue("@ID", idIgnorar);
                    return Convert.ToInt64(cmd.ExecuteScalar());
                }
            }
        }

        // ----- senha -----

        /// <summary>
        /// PBKDF2-SHA256 com salt aleatorio. Formato: PBKDF2$iteracoes$salt$hash
        /// </summary>
        private static string GerarHash(string senha)
        {
            byte[] salt = new byte[TamanhoSalt];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = Derivar(senha, salt, Iteracoes);

            return string.Format("PBKDF2${0}${1}${2}",
                Iteracoes, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        private static bool SenhaConfere(string senha, string armazenado)
        {
            if (string.IsNullOrEmpty(armazenado))
                return false;

            string[] partes = armazenado.Split('$');
            if (partes.Length != 4 || partes[0] != "PBKDF2")
                return false;

            int iteracoes;
            if (!int.TryParse(partes[1], out iteracoes) || iteracoes <= 0)
                return false;

            byte[] salt;
            byte[] esperado;
            try
            {
                salt = Convert.FromBase64String(partes[2]);
                esperado = Convert.FromBase64String(partes[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] calculado = Derivar(senha ?? "", salt, iteracoes);

            return IguaisEmTempoFixo(esperado, calculado);
        }

        private static byte[] Derivar(string senha, byte[] salt, int iteracoes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(senha ?? "", salt, iteracoes, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(TamanhoHash);
            }
        }

        private static bool IguaisEmTempoFixo(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diferenca = 0;
            for (int i = 0; i < a.Length; i++)
                diferenca |= a[i] ^ b[i];

            return diferenca == 0;
        }
    }
}
