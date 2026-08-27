using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Portaria
{
    /// <summary>
    /// Ajusta a estrutura do banco na abertura do sistema.
    ///
    /// Bancos antigos foram criados sem as colunas ID e SAIDA na tabela VEICULO,
    /// que o sistema usa para registrar a saida e localizar a linha selecionada.
    /// Sem elas as consultas falham com "no such column: ID".
    /// </summary>
    public static class Banco
    {
        // Mesma origem de dados usada pelo restante do sistema.
        private const string Conexao = @"Data Source=ControleAcesso.db;";

        /// <summary>
        /// Colunas da tabela VEICULO na ordem esperada pelo sistema, com os
        /// nomes antigos que devem ser aproveitados quando existirem.
        /// </summary>
        private static readonly ColunaDestino[] ColunasVeiculo =
        {
            new ColunaDestino("CPF",          "TEXT", "CPF"),
            new ColunaDestino("NOME",         "TEXT", "NOME"),
            new ColunaDestino("CELULAR",      "TEXT", "CELULAR"),
            new ColunaDestino("CPFAJUDANTE",  "TEXT", "CPFAJUDANTE", "CPF AJUDANTE"),
            new ColunaDestino("NOMEAJUDANTE", "TEXT", "NOMEAJUDANTE", "NOME AJUDANTE"),

            // Ajudantes 2 a 5. O ajudante 1 continua em CPFAJUDANTE/NOMEAJUDANTE:
            // os 22 mil registros antigos e todas as consultas que ja liam essas
            // duas colunas seguem funcionando sem conversao.
            new ColunaDestino("CPFAJUDANTE2",  "TEXT", "CPFAJUDANTE2"),
            new ColunaDestino("NOMEAJUDANTE2", "TEXT", "NOMEAJUDANTE2"),
            new ColunaDestino("CPFAJUDANTE3",  "TEXT", "CPFAJUDANTE3"),
            new ColunaDestino("NOMEAJUDANTE3", "TEXT", "NOMEAJUDANTE3"),
            new ColunaDestino("CPFAJUDANTE4",  "TEXT", "CPFAJUDANTE4"),
            new ColunaDestino("NOMEAJUDANTE4", "TEXT", "NOMEAJUDANTE4"),
            new ColunaDestino("CPFAJUDANTE5",  "TEXT", "CPFAJUDANTE5"),
            new ColunaDestino("NOMEAJUDANTE5", "TEXT", "NOMEAJUDANTE5"),
            new ColunaDestino("DataHora",     "TEXT", "DATAHORA", "DATA / HORA", "DATA/HORA"),
            new ColunaDestino("SAIDA",        "TEXT", "SAIDA", "SAÍDA"),
            new ColunaDestino("PLACA",        "TEXT", "PLACA"),
            new ColunaDestino("TIPOVEICULO",  "TEXT", "TIPOVEICULO", "TIPO VEICULO"),
            new ColunaDestino("PRESTADOR",    "TEXT", "PRESTADOR"),
            new ColunaDestino("AGREGADO",     "TEXT", "AGREGADO"),
            new ColunaDestino("EMPRESA",      "TEXT", "EMPRESA"),
            new ColunaDestino("USUARIOENTRADA", "TEXT", "USUARIOENTRADA", "USUARIO ENTRADA")
        };

        private class ColunaDestino
        {
            public readonly string Nome;
            public readonly string Tipo;
            public readonly string[] NomesAntigos;

            public ColunaDestino(string nome, string tipo, params string[] nomesAntigos)
            {
                Nome = nome;
                Tipo = tipo;
                NomesAntigos = nomesAntigos;
            }
        }

        public static void GarantirEstrutura()
        {
            using (var con = new SQLiteConnection(Conexao))
            {
                con.Open();

                GarantirAgendamento(con);
                GarantirMercadoria(con);
                GarantirVeiculo(con);
            }
        }

        private static void GarantirAgendamento(SQLiteConnection con)
        {
            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS AGENDAMENTO (
                    USUARIO TEXT,
                    NOME    TEXT,
                    EMPRESA TEXT,
                    DATAHORA TEXT,
                    field5  TEXT
                )";
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Colunas da tabela MERCADORIA: os dados da chegada e, quando o
        /// destinatario retira, quem levou (RETIRADOPOR) com data e hora.
        /// </summary>
        private static readonly ColunaDestino[] ColunasMercadoria =
        {
            new ColunaDestino("DATAHORA",        "TEXT"),
            new ColunaDestino("DESTINATARIO",    "TEXT"),
            new ColunaDestino("EMPRESA",         "TEXT"),
            new ColunaDestino("ENTREGADOR",      "TEXT"),
            new ColunaDestino("RECEBEDOR",       "TEXT"),
            new ColunaDestino("USUARIOREGISTRO", "TEXT"),
            new ColunaDestino("ENTREGUE",        "TEXT"),
            new ColunaDestino("RETIRADOPOR",     "TEXT"),
            new ColunaDestino("DATAENTREGA",     "TEXT"),
            new ColunaDestino("USUARIOENTREGA",  "TEXT")
        };

        /// <summary>
        /// Tabela das mercadorias que chegam na portaria. Uma linha por chegada,
        /// no mesmo modelo de VEICULO: nao ha cadastro de fornecedor.
        /// Bancos que ja tenham a tabela recebem apenas as colunas que faltarem.
        /// </summary>
        private static void GarantirMercadoria(SQLiteConnection con)
        {
            List<string> colunas = Colunas(con, "MERCADORIA");

            if (colunas.Count == 0)
            {
                var sql = new System.Text.StringBuilder();
                sql.Append("CREATE TABLE MERCADORIA (ID INTEGER PRIMARY KEY AUTOINCREMENT");

                foreach (ColunaDestino coluna in ColunasMercadoria)
                    sql.AppendFormat(", [{0}] {1}", coluna.Nome, coluna.Tipo);

                sql.Append(")");

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql.ToString();
                    cmd.ExecuteNonQuery();
                }

                return;
            }

            foreach (ColunaDestino coluna in ColunasMercadoria)
            {
                if (Contem(colunas, coluna.Nome))
                    continue;

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = string.Format(
                        "ALTER TABLE MERCADORIA ADD COLUMN [{0}] {1}", coluna.Nome, coluna.Tipo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void GarantirVeiculo(SQLiteConnection con)
        {
            List<string> colunas = Colunas(con, "VEICULO");

            // Tabela ainda nao existe: cria ja no formato correto.
            if (colunas.Count == 0)
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = SqlCriarVeiculo("VEICULO");
                    cmd.ExecuteNonQuery();
                }
                return;
            }

            // Com o ID no lugar, o que faltar (SAIDA, USUARIOENTRADA...) entra
            // com ALTER TABLE, sem refazer a tabela.
            if (Contem(colunas, "ID"))
            {
                AcrescentarColunasFaltantes(con, colunas);
                return;
            }

            // Falta o ID. O SQLite nao permite acrescentar uma chave primaria
            // AUTOINCREMENT, entao a tabela e refeita e os dados sao copiados.
            // A tabela antiga NAO e apagada: fica guardada como VEICULO_ANTIGA.
            string backup = NomeLivre(con, "VEICULO_ANTIGA");

            using (var tran = con.BeginTransaction())
            using (var cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;

                cmd.CommandText = string.Format("ALTER TABLE VEICULO RENAME TO {0}", backup);
                cmd.ExecuteNonQuery();

                cmd.CommandText = SqlCriarVeiculo("VEICULO");
                cmd.ExecuteNonQuery();

                // Copia somente as colunas que existiam na tabela antiga.
                List<string> destino = new List<string>();
                List<string> origem = new List<string>();

                foreach (ColunaDestino coluna in ColunasVeiculo)
                {
                    string antiga = Equivalente(colunas, coluna);
                    if (antiga == null) continue;

                    destino.Add("[" + coluna.Nome + "]");
                    origem.Add("[" + antiga + "]");
                }

                if (destino.Count > 0)
                {
                    cmd.CommandText = string.Format(
                        "INSERT INTO VEICULO ({0}) SELECT {1} FROM {2}",
                        string.Join(", ", destino.ToArray()),
                        string.Join(", ", origem.ToArray()),
                        backup);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
        }

        /// <summary>
        /// Acrescenta em VEICULO as colunas esperadas que ainda nao existem.
        /// </summary>
        private static void AcrescentarColunasFaltantes(SQLiteConnection con, List<string> colunas)
        {
            foreach (ColunaDestino coluna in ColunasVeiculo)
            {
                if (Contem(colunas, coluna.Nome))
                    continue;

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = string.Format(
                        "ALTER TABLE VEICULO ADD COLUMN [{0}] {1}", coluna.Nome, coluna.Tipo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string SqlCriarVeiculo(string nomeTabela)
        {
            var sql = new System.Text.StringBuilder();
            sql.AppendFormat("CREATE TABLE {0} (", nomeTabela);
            sql.Append("ID INTEGER PRIMARY KEY AUTOINCREMENT");

            foreach (ColunaDestino coluna in ColunasVeiculo)
                sql.AppendFormat(", [{0}] {1}", coluna.Nome, coluna.Tipo);

            sql.Append(")");
            return sql.ToString();
        }

        private static List<string> Colunas(SQLiteConnection con, string tabela)
        {
            var nomes = new List<string>();

            using (var cmd = con.CreateCommand())
            {
                cmd.CommandText = string.Format("PRAGMA table_info({0})", tabela);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        nomes.Add(dr["name"].ToString());
                }
            }

            return nomes;
        }

        private static bool Contem(List<string> colunas, string nome)
        {
            foreach (string coluna in colunas)
            {
                if (string.Equals(coluna, nome, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Nome real da coluna antiga que corresponde ao destino, ou null.</summary>
        private static string Equivalente(List<string> colunas, ColunaDestino destino)
        {
            foreach (string candidato in destino.NomesAntigos)
            {
                foreach (string coluna in colunas)
                {
                    if (string.Equals(coluna, candidato, StringComparison.OrdinalIgnoreCase))
                        return coluna;
                }
            }
            return null;
        }

        /// <summary>Primeiro nome de tabela ainda nao usado: base, base_2, base_3...</summary>
        private static string NomeLivre(SQLiteConnection con, string baseNome)
        {
            for (int i = 1; i < 100; i++)
            {
                string nome = i == 1 ? baseNome : baseNome + "_" + i;

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name = @NOME";
                    cmd.Parameters.AddWithValue("@NOME", nome);

                    if (Convert.ToInt64(cmd.ExecuteScalar()) == 0)
                        return nome;
                }
            }

            throw new InvalidOperationException("Não foi possível criar a cópia de segurança da tabela VEICULO.");
        }
    }
}
