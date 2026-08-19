# PORTARIA WEB — Especificação para construção do projeto

Documento de referência para reconstruir o sistema **Portaria** (hoje WinForms desktop) como aplicação web.
Levantado a partir do código em `PORTARIA/Portaria/` e do banco de produção
`PORTARIA/Portaria/bin/Debug/controleAcesso.db` em **11/08/2026**.

> **Premissa assumida:** stack .NET (o time já é C#). A seção 9 traz a stack recomendada e as
> alternativas. As seções 1 a 8 são independentes de stack e valem para qualquer tecnologia.

---

## 1. O que o sistema faz

Controle de acesso de veículos e pessoas na portaria de uma empresa. O porteiro:

1. Digita a **placa** ou o **documento** (RG/CPF) de quem está chegando.
2. O sistema busca a última visita daquela placa/documento e **pré-preenche** motorista, celular,
   ajudante, tipo de veículo e empresa — para não redigitar tudo a cada visita.
3. Completa o que falta e **salva a entrada** (grava data/hora e quem registrou).
4. Quando o veículo sai, seleciona a linha na grade do dia e clica em **SAÍDA** (grava data/hora de saída).
5. A qualquer momento gera **relatórios em PDF** por período, com escolha de colunas e filtro por porteiro.

Cadastro de usuários (porteiros) com dois níveis de acesso. Existe também um módulo de
**agendamento** de visitas, hoje praticamente desativado (ver 4.6).

### Volume real de uso

| Métrica | Valor |
|---|---|
| Registros de acesso (`Veiculo`) | **22.175** |
| Registros com data/hora preenchida | 10.897 (os outros 11.278 são importação legada sem data) |
| Período com data/hora | 02/12/2025 a 03/08/2026 |
| Entradas sem saída registrada (dentre as que têm data) | 1.208 |
| Placas distintas | 10.368 |
| Nomes de motorista distintos | 10.864 |
| Documentos (CPF/RG) distintos | 12.560 |
| Empresas distintas | 3.608 |
| Registros com ajudante | 2.722 |
| Usuários cadastrados | 3 (`admin`, `GUIMARAES` nível 1; `TESTE` nível 2) |
| Tamanho do banco | ~28 MB |

Ordem de grandeza: **~1.500 entradas/mês**, tráfego de um punhado de porteiros simultâneos.
Não é um sistema de alto volume — a complexidade está nas regras e nos dados sujos, não na escala.

---

## 2. Inventário do sistema atual

Projeto: `PORTARIA/Portaria/Portaria.csproj` — WinForms, **.NET Framework 4.8**, `packages.config`
(NuGet clássico, não SDK-style). Solution: `PORTARIA/Portaria.sln`.

| Arquivo | Linhas | Responsabilidade |
|---|---|---|
| `Frm_Veiculo.cs` | 869 | Tela principal: entrada, busca, saída, grade do dia, histórico |
| `Frm_Usuarios.cs` | 428 | CRUD de usuários, troca de senha, ativar/inativar |
| `Frm_relatorio_personalizado.cs` | 406 | Relatório PDF com escolha de colunas e filtro por usuário |
| `Seguranca.cs` | 373 | `Nivel`, `UsuarioInfo`, `Sessao`, `Usuarios` (autenticação e hash) |
| `Mascaras.cs` | 317 | `Placa`, `Documento` (CPF), máscaras dos campos |
| `Banco.cs` | 241 | Migração automática do schema na abertura |
| `Frm_relatorio_data.cs` | 223 | Relatório PDF fixo de 10 colunas por período |
| `Frm_Agendamento.cs` | 157 | Agendamento de visitas (módulo dormente) |
| `DataAccess.cs` | 94 | Classes POCO geradas por ferramenta — **código morto** |
| `Frm_Login.cs` | 73 | Login |
| `Program.cs` | 68 | Entry point: migra banco → login → tela principal |
| `Frm_Cadastro.cs` | 30 | Form vazio — **código morto** |

Dependências relevantes (`packages.config`):

- `System.Data.SQLite` (Stub.System.Data.SQLite.Core.NetFramework 1.0.119) — acesso a dados
- `QuestPDF` 2023.12.6 (licença Community) — geração dos PDFs
- `ClosedXML` 0.105 + `DocumentFormat.OpenXml` — Excel (não usado nos fluxos ativos)
- `SkiaSharp` / `HarfBuzzSharp` — renderização do QuestPDF
- `Microsoft.Data.SqlClient` 6.1.3 — presente, mas **nenhum código usa SQL Server hoje**

### Como o desktop conversa com o banco

Não há camada de dados. Cada form abre sua própria `SQLiteConnection` com a string literal
`Data Source=ControleAcesso.db;` e monta o SQL inline. O banco é um arquivo **relativo ao diretório
de trabalho do executável** — na prática `bin/Debug/controleAcesso.db`.

Consequência direta para a web: **não há nada para reaproveitar em termos de arquitetura**. O que
se reaproveita é (a) as regras de validação de `Mascaras.cs`, (b) o algoritmo de senha de
`Seguranca.cs`, (c) os SQLs de consulta como especificação de comportamento.

---

## 3. Modelo de dados atual (real, extraído do banco)

### 3.1 `Veiculo` — o registro de acesso (22.175 linhas)

```sql
CREATE TABLE "Veiculo" (
    ID             INTEGER PRIMARY KEY AUTOINCREMENT,
    CPF            TEXT,   -- documento do motorista (RG *ou* CPF)
    NOME           TEXT,   -- nome do motorista
    CELULAR        TEXT,
    CPFAJUDANTE    TEXT,
    NOMEAJUDANTE   TEXT,
    DataHora       TEXT,   -- entrada: 'yyyy-MM-dd HH:mm:ss'
    SAIDA          TEXT,   -- saída:   'dd/MM/yyyy HH:mm:ss'  (!! formato diferente)
    PLACA          TEXT,
    TIPOVEICULO    TEXT,   -- texto livre
    PRESTADOR      TEXT,   -- 'SIM' / 'NAO' / '' / NULL
    AGREGADO       TEXT,   -- 'SIM' / 'NÃO' / '' / NULL
    EMPRESA        TEXT,
    USUARIOENTRADA TEXT    -- login de quem registrou (coluna nova)
);
```

Uma linha = **um evento de entrada**. Não existe cadastro de pessoas nem de veículos: os dados do
motorista são recopiados a cada visita. É por isso que há 10.368 placas e 12.560 documentos
distintos em 22 mil registros.

### 3.2 `USUARIO` (3 linhas)

```sql
CREATE TABLE USUARIO (
    ID    INTEGER PRIMARY KEY AUTOINCREMENT,
    LOGIN TEXT NOT NULL COLLATE NOCASE UNIQUE,
    NOME  TEXT,
    SENHA TEXT NOT NULL,   -- 'PBKDF2$<iteracoes>$<salt b64>$<hash b64>'
    NIVEL INTEGER NOT NULL DEFAULT 2,
    ATIVO INTEGER NOT NULL DEFAULT 1
);
```

### 3.3 `AGENDAMENTO` (14 linhas)

```sql
CREATE TABLE "AGENDAMENTO" (
    USUARIO TEXT, NOME TEXT, EMPRESA TEXT, DATAHORA TEXT, field5 TEXT
);
```

`USUARIO` guarda o **usuário do Windows** (`Environment.UserName`), não o login do sistema.
`field5` nunca é escrito.

### 3.4 `CONTROLE` (37.534 linhas) — **não migrar**

Planilha antiga importada crua em 255 colunas genéricas: `field1`=empresa, `field2`=hora de entrada,
`field3`=motorista, `field4`=documento, `field5`/`field6`=ajudante, `field7`=placa, `field8`=tipo,
`field9`=hora de saída, `field10`=porteiro. **Não tem data**, só hora (`05:45`), e o texto está com
encoding quebrado (`JOSU?` = `JOSUÉ`). Cerca de 27.169 dessas linhas já foram importadas para
`Veiculo` (são as ~11.278 linhas com `DataHora` vazia). Nenhum código lê essa tabela.

**Decisão:** deixar `CONTROLE` fora do projeto web. Se um dia for necessária, é um trabalho de
arqueologia de dados separado — exige achar a data no arquivo original.

### 3.5 Sujeira de dados que a web tem de tolerar

Isto é a parte mais importante deste documento. Qualquer modelagem que ignore estes pontos quebra
com os dados reais.

| Campo | Problema | Impacto |
|---|---|---|
| `PLACA` | ~7.700 de 10.368 gravadas **com espaço** (`GCT 6604`), o resto sem (`EQH9J73`); 4 com hífen; 93 fora do padrão (erros de digitação como `AUKOE50` com letra O no lugar do zero, e 4 `S/ PLACA`) | Busca com `=` simples acha metade dos registros. **Obrigatório** comparar por valor normalizado |
| `CPF` | Gravado cru (só dígitos), mas ~200 registros vêm com pontuação. Dos 1.544 CPFs de 11 dígitos, **48 têm dígito verificador inválido** | Validação de CPF aplicada retroativamente barra dados legados existentes |
| `DataHora` vs `SAIDA` | Entrada em `yyyy-MM-dd HH:mm:ss`, saída em `dd/MM/yyyy HH:mm:ss` | `SAIDA` não é ordenável nem comparável em SQL; impossível calcular permanência |
| `DataHora` | 11.278 linhas vazias (importação legada) | Todo filtro por período tem de tratar o vazio |
| `PRESTADOR` | Quatro valores: `SIM`, `NAO`, `''`, `NULL` | Não é booleano |
| `AGREGADO` | Quatro valores: `SIM`, `NÃO` (com til!), `''`, `NULL` | Idem, e o til difere de `PRESTADOR` |
| `TIPOVEICULO` | Texto livre. Top valores: FIORINO (5.163), DAILY (1.708), HR (1.664), TRUCK (1.436), DUCATO (1.125), TOCO (946), DOBLO (921), **¾ (915)**, CARRETA (870), MASTER (811), IVECO (708), SPRINTER (553), KOMBI (414), **3/4. (321)**, **3/4 (220)** | `¾`, `3/4` e `3/4.` são o mesmo tipo escrito de 3 formas. Precisa de tabela de tipos + de-para |
| `USUARIOENTRADA` | Apenas **2** registros preenchidos (coluna recente) | Filtro por porteiro é inútil no histórico; o relatório já trata isso com a opção "(SEM USUARIO REGISTRADO)" |
| `EMPRESA` | Texto livre, 3.608 valores distintos, com espaços nas pontas (o código faz `.Trim()` na leitura e na escrita) | Consolidação de empresas é um projeto em si |
| Encoding | Registros vindos da planilha têm acentuação corrompida | Corrigir na migração, não em runtime |

---

## 4. Regras de negócio (com origem no código)

### 4.1 Placa — `Mascaras.cs:17-122`

- Dois formatos brasileiros: antigo `LLL9999` e Mercosul `LLL9L99`. Sempre **7 caracteres, sem separador**.
- Validação por posição: 0-2 letra, 3 dígito, **4 letra ou dígito** (é o que distingue os dois formatos), 5-6 dígito.
- `Placa.Aplicar()` — descarta o que não encaixa (usado em texto colado).
- `Placa.Normalizar()` — reduz a letras e dígitos maiúsculos; **é o que permite achar os registros antigos com espaço**.
- Na gravação (`Frm_Veiculo.cs:229-230`): se o texto formar placa completa, grava normalizado; se não
  (ex.: `S/ PLACA`), grava como o porteiro digitou. **Manter esse comportamento** — a portaria precisa
  registrar veículo sem placa.
- Nas consultas, a coluna é normalizada em SQL (`Frm_Veiculo.cs:32-33`):
  ```sql
  REPLACE(REPLACE(REPLACE(REPLACE(UPPER(IFNULL(PLACA,'')),' ',''),'-',''),'/',''),'.','')
  ```
  Na web isto deve virar uma **coluna persistida `placa_normalizada` com índice** — normalizar em
  runtime impede o uso de índice e faz varredura completa a cada busca.

### 4.2 Documento — `Mascaras.cs:128-197`

- O campo aceita **RG ou CPF**. A regra: 11 dígitos e nada mais ⇒ é CPF ⇒ o dígito verificador
  (módulo 11) tem de fechar. Qualquer outra coisa (RG, menos dígitos, letras) passa sem validação.
- Rejeita CPFs de dígitos repetidos (`00000000000`).
- Validação em **três momentos**: ao digitar (bloqueia a tecla + beep + tooltip, `Mascaras.cs:262`),
  ao mudar o texto (fundo rosa `#FFCDCD`, `Mascaras.cs:280`) e ao salvar (`Frm_Veiculo.cs:310-323`).
- **Atenção:** existem 48 CPFs legados com DV inválido no banco. Se a web validar na leitura, esses
  registros ficam ineditáveis. Validar só na **escrita de dados novos**.

### 4.3 Entrada de acesso — `Frm_Veiculo.cs:205-270`

Obrigatórios: `NOME` e `RG/CPF`. Todo o resto é opcional (inclusive a placa).
Validação de CPF do motorista **e de todos os ajudantes** antes de gravar (`Frm_Veiculo.cs:275-297`).
Grava `DataHora = agora` (`yyyy-MM-dd HH:mm:ss`), `SAIDA = ''`, `USUARIOENTRADA = login da sessão`.

**Limitação conhecida a corrigir na web:** a tela permite criar N abas de ajudante em tempo de
execução (`Frm_Veiculo.cs:656-712`), mas o INSERT só grava **um** ajudante — `CPFAJUDANTE`/
`NOMEAJUDANTE` da primeira aba. Os ajudantes 2, 3… são digitados, validados e **descartados**.
A web deve modelar ajudantes como coleção (ver 6.2).

### 4.4 Busca e pré-preenchimento

Dois caminhos, ambos trazendo **a visita mais recente** (`ORDER BY DataHora DESC LIMIT 1`):

- **Por placa** (`Frm_Veiculo.cs:373-426`): compara placa normalizada. Preenche documento, nome,
  celular, ajudante, tipo, empresa. **Não** preenche a placa (já está digitada).
- **Por documento** (`Frm_Veiculo.cs:587-634`): compara `CPF` exato em maiúsculas — **sem
  normalização**, então não acha os ~200 registros gravados com pontuação. Corrigir na web:
  normalizar o documento igual à placa.

Achando ou não, abre o **histórico** daquela placa/documento (`Frm_Veiculo.cs:482-549`): lista de
`ENTRADA` + `SAIDA` de todas as visitas, ordenada por data desc.

### 4.5 Grade do dia e saída

- Grade "ÚLTIMAS VISITAS" (`Frm_Veiculo.cs:325-369`): `WHERE DATE(DataHora) = DATE('now')`,
  ordenada por `DataHora DESC`. Coluna `ID` oculta.
- Um `Timer` de **5 segundos** (`Frm_Veiculo.Designer.cs:425`) recarrega a grade inteira. Na web,
  trocar por push (SignalR/WebSocket) ou polling mais espaçado — recarregar tudo a cada 5s por
  usuário é desperdício.
- Checkbox "OCULTAR SAÍDAS" (`Frm_Veiculo.cs:849-862`): filtro **client-side** sobre o `DataTable`
  (`SAIDA IS NULL OR SAIDA = ''`). Volta marcado a cada atualização. Na web, virar filtro de query.
- Botão SAÍDA (`Frm_Veiculo.cs:727-777`): exige linha selecionada, pede confirmação, faz
  `UPDATE Veiculo SET SAIDA = @saida WHERE ID = @id` com `dd/MM/yyyy HH:mm:ss`.
  **Não impede registrar saída duas vezes** nem valida se a saída é posterior à entrada.

### 4.6 Agendamento — dormente

A grade "AGENDAMENTO DO DIA" está desligada por uma flag de código
(`Frm_Veiculo.cs:22`: `MostrarAgendamentoDoDia = false`). O `Frm_Agendamento` existe, insere e
consulta por `Environment.UserName`, mas não é alcançável pelo menu da tela principal.
14 registros no banco.

**Decisão a tomar com o usuário:** o agendamento entra no escopo da web (repensado, com portaria
vendo quem é esperado hoje) ou fica de fora da primeira versão? Recomendação: **fora da fase 1**,
retomado depois com requisito próprio.

### 4.7 Relatórios

**Relatório por data** (`Frm_relatorio_data.cs`) — 10 colunas fixas (CPF, nome, celular, CPF
ajudante, nome ajudante, entrada, saída, placa, prestador, empresa), período por dois date pickers,
A4 retrato, rodapé com total e data de geração. Salva em `relatorio.pdf` na pasta do executável e
abre no Explorer.

**Relatório personalizado** (`Frm_relatorio_personalizado.cs`) — mais completo, é o modelo a seguir:

- Checklist com **13 campos** (todas as colunas de `Veiculo`), todos marcados por padrão, com
  botões marcar/desmarcar tudo. As expressões SQL são **fixas no código** (`CamposDisponiveis`,
  linhas 41-56) — nada de SQL vindo do usuário.
- Período padrão: primeiro dia do mês corrente até hoje. Valida final ≥ inicial.
  Limite superior **exclusivo** (`fim.AddDays(1)`).
- Filtro "somente sem saída".
- Filtro por porteiro: combo com TODOS + usuários cadastrados + logins que aparecem em
  `USUARIOENTRADA` mas não existem mais no cadastro (marcados `(REMOVIDO)`) + `(SEM USUARIO REGISTRADO)`.
- Vira **paisagem** automaticamente com mais de 6 colunas.
- Trata o PDF já aberto ("Feche o relatório que já está aberto e gere novamente").

Na web: gerar o PDF server-side e devolver como download, ou renderizar em HTML com opção de
exportar PDF/Excel. O `QuestPDF` funciona em ASP.NET Core — o código de layout é reaproveitável quase
literalmente.

### 4.8 Autenticação e permissões — `Seguranca.cs`

- Senha: **PBKDF2-SHA256, 50.000 iterações, salt aleatório de 16 bytes, hash de 32 bytes**.
  Formato armazenado: `PBKDF2$50000$<salt b64>$<hash b64>`. Comparação em tempo constante
  (`Seguranca.cs:361`).
  **Este formato é portável** — a web pode reusar os hashes existentes sem forçar troca de senha.
- Dois níveis (`Seguranca.cs:11-20`): **1 = acesso total** (inclui cadastro de usuários),
  **2 = restrito** (tudo menos cadastro de usuários). Não há mais granularidade.
- Login inválido: mensagem única "Usuário ou senha inválidos" — não revela qual campo errou.
  Só autentica `ATIVO = 1`.
- Na primeira execução cria `admin` / `admin` nível 1 (`Seguranca.cs:87-98`).
  **Na web, forçar troca de senha no primeiro login.**
- Sem login validado o app não abre (`Program.cs:36-41`). Sessão em `static Sessao.Atual`.
- Proteção de permissão em duas camadas: o menu fica oculto para nível 2 **e** a ação revalida
  (`Frm_Veiculo.cs:105-120`).
- `Usuarios.OutrosAdministradoresAtivos()` (`Seguranca.cs:287`) impede o sistema ficar sem nenhum
  administrador ativo. **Manter essa regra.**
- Senha mínima de 4 caracteres (`Frm_Usuarios.cs:14`) — **aumentar para 8+ na web**.
- Alteração de usuário: senha em branco mantém a atual (`Frm_Usuarios.cs:113`).

### 4.9 Comportamentos de UI a preservar

O sistema é operado por porteiro, com pressa, muitas vezes só com teclado:

- **Tudo em maiúsculas.** Aplicado recursivamente a todos os `TextBox` de todos os forms
  (`Frm_Veiculo.cs:54-63`). Na web: `text-transform: uppercase` + normalização no servidor.
- **Enter dispara a busca certa.** O botão default muda conforme o campo em foco
  (`Frm_Veiculo.cs:152-155`, `195-198`): digitando no documento, Enter busca por documento;
  digitando na placa, Enter busca por placa.
- Máscara de placa rejeita a tecla inválida **com beep** em vez de aceitar e reclamar depois.
- Confirmação antes de registrar saída.
- Grade do dia com fonte grande (Segoe UI 12) — leitura à distância, no balcão.

---

## 5. Escopo da versão web

### 5.1 Dentro do escopo (fase 1 — paridade)

| # | Caso de uso | Origem |
|---|---|---|
| 1 | Login com usuário/senha, sessão, logout | `Frm_Login` |
| 2 | Registrar entrada, com N ajudantes | `Frm_Veiculo` (corrigindo 4.3) |
| 3 | Buscar por placa (normalizada) e pré-preencher | `Frm_Veiculo:373` |
| 4 | Buscar por documento (normalizado) e pré-preencher | `Frm_Veiculo:587` |
| 5 | Histórico de visitas de uma placa/documento | `Frm_Veiculo:482` |
| 6 | Grade de movimentação do dia, atualizada em tempo real | `Frm_Veiculo:325` |
| 7 | Registrar saída com confirmação | `Frm_Veiculo:727` |
| 8 | Filtro "somente quem está dentro" | `OcultarVisitas` |
| 9 | Relatório por período com escolha de colunas → PDF | `Frm_relatorio_personalizado` |
| 10 | CRUD de usuários, níveis, ativar/inativar, troca de senha | `Frm_Usuarios` |

### 5.2 Ganhos que só a web viabiliza (fase 2)

- **Multi-portaria / multi-guarita** — o desktop é um arquivo SQLite por máquina.
- **Cadastro de pessoas e veículos** de verdade (ver 6.2): a busca deixa de ser "última visita" e
  passa a ser cadastro consultado.
- **Dashboard**: quem está dentro agora, tempo médio de permanência, picos por hora, top empresas.
- **Auditoria**: quem alterou o quê e quando (hoje não existe).
- **Exportação Excel** (o `ClosedXML` já está no projeto, sem uso).
- **Foto** do motorista/veículo, leitura de placa por câmera, QR code de convite.
- Acesso do porteiro por tablet na guarita.

### 5.3 Fora do escopo

- Migração da tabela `CONTROLE` (3.4).
- Módulo de agendamento (4.6) — decisão pendente.
- Integração com catraca/cancela — não existe hoje.

---

## 6. Modelo de dados proposto

### 6.1 Princípio

O modelo atual é uma tabela achatada de eventos. O modelo web separa **cadastro** (pessoa, veículo,
empresa) de **movimento** (o acesso). Isso resolve de uma vez a duplicação, o pré-preenchimento e a
consolidação de empresas.

Mas **a migração não pode perder nada**: os 22.175 registros existentes viram acessos, e as
pessoas/veículos são deduzidos deles.

### 6.2 Tabelas

```sql
-- ---------- CADASTRO ----------

CREATE TABLE empresa (
    id            BIGSERIAL PRIMARY KEY,
    nome          TEXT NOT NULL,
    nome_norm     TEXT NOT NULL,           -- maiúsculas, sem acento, trim
    cnpj          TEXT NULL,
    criado_em     TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (nome_norm)
);

CREATE TABLE pessoa (
    id            BIGSERIAL PRIMARY KEY,
    documento     TEXT NOT NULL,           -- como digitado
    documento_norm TEXT NOT NULL,          -- só dígitos/letras, maiúsculas
    tipo_documento TEXT NOT NULL,          -- 'CPF' | 'RG' | 'OUTRO'
    nome          TEXT NOT NULL,
    celular       TEXT NULL,
    empresa_id    BIGINT NULL REFERENCES empresa(id),
    observacao    TEXT NULL,
    ativo         BOOLEAN NOT NULL DEFAULT true,
    criado_em     TIMESTAMPTZ NOT NULL DEFAULT now(),
    UNIQUE (documento_norm)
);
CREATE INDEX ix_pessoa_nome ON pessoa (nome);

CREATE TABLE veiculo (
    id            BIGSERIAL PRIMARY KEY,
    placa         TEXT NOT NULL,           -- como gravada/digitada (aceita 'S/ PLACA')
    placa_norm    TEXT NOT NULL,           -- só letras e dígitos, maiúsculas  << índice de busca
    tipo_id       BIGINT NULL REFERENCES tipo_veiculo(id),
    criado_em     TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX ix_veiculo_placa_norm ON veiculo (placa_norm);

CREATE TABLE tipo_veiculo (                -- resolve '¾' vs '3/4' vs '3/4.'
    id            BIGSERIAL PRIMARY KEY,
    nome          TEXT NOT NULL UNIQUE,    -- 'FIORINO', 'TRUCK', '3/4', ...
    ativo         BOOLEAN NOT NULL DEFAULT true
);

CREATE TABLE tipo_veiculo_alias (          -- de-para da migração e da digitação livre
    alias         TEXT PRIMARY KEY,        -- '¾', '3/4.', 'TRÊS QUARTOS'
    tipo_id       BIGINT NOT NULL REFERENCES tipo_veiculo(id)
);

-- ---------- MOVIMENTO ----------

CREATE TABLE acesso (
    id                BIGSERIAL PRIMARY KEY,
    pessoa_id         BIGINT NOT NULL REFERENCES pessoa(id),
    veiculo_id        BIGINT NULL REFERENCES veiculo(id),
    empresa_id        BIGINT NULL REFERENCES empresa(id),

    -- snapshot: o que foi digitado no momento do acesso, imutável.
    -- Mesmo que a pessoa mude de nome/empresa depois, o registro histórico não muda.
    nome_snapshot     TEXT NOT NULL,
    documento_snapshot TEXT NOT NULL,
    placa_snapshot    TEXT NULL,
    empresa_snapshot  TEXT NULL,
    tipo_veiculo_snapshot TEXT NULL,

    entrada_em        TIMESTAMPTZ NULL,    -- NULL = registro legado sem data
    saida_em          TIMESTAMPTZ NULL,    -- NULL = ainda dentro
    prestador         BOOLEAN NULL,        -- NULL preservado (dado legado ausente)
    agregado          BOOLEAN NULL,

    usuario_entrada_id BIGINT NULL REFERENCES usuario(id),
    usuario_entrada_login TEXT NULL,       -- preserva login de usuário já removido
    usuario_saida_id  BIGINT NULL REFERENCES usuario(id),

    origem            TEXT NOT NULL DEFAULT 'WEB',  -- 'WEB' | 'MIGRACAO_DESKTOP'
    legado_id         BIGINT NULL,         -- Veiculo.ID original, para rastrear a migração
    criado_em         TIMESTAMPTZ NOT NULL DEFAULT now()
);
CREATE INDEX ix_acesso_entrada     ON acesso (entrada_em DESC);
CREATE INDEX ix_acesso_dentro      ON acesso (saida_em) WHERE saida_em IS NULL;
CREATE INDEX ix_acesso_pessoa      ON acesso (pessoa_id, entrada_em DESC);
CREATE INDEX ix_acesso_veiculo     ON acesso (veiculo_id, entrada_em DESC);

CREATE TABLE acesso_acompanhante (         -- resolve a limitação dos N ajudantes
    id            BIGSERIAL PRIMARY KEY,
    acesso_id     BIGINT NOT NULL REFERENCES acesso(id) ON DELETE CASCADE,
    pessoa_id     BIGINT NULL REFERENCES pessoa(id),
    documento_snapshot TEXT NULL,
    nome_snapshot TEXT NOT NULL,
    ordem         INT NOT NULL DEFAULT 1
);
CREATE INDEX ix_acompanhante_acesso ON acesso_acompanhante (acesso_id);

-- ---------- SEGURANÇA ----------

CREATE TABLE usuario (
    id             BIGSERIAL PRIMARY KEY,
    login          TEXT NOT NULL,
    login_norm     TEXT NOT NULL UNIQUE,   -- equivale ao COLLATE NOCASE do SQLite
    nome           TEXT NULL,
    senha_hash     TEXT NOT NULL,          -- 'PBKDF2$50000$salt$hash' (compatível com o desktop)
    nivel          INT NOT NULL DEFAULT 2, -- 1 = total, 2 = restrito
    ativo          BOOLEAN NOT NULL DEFAULT true,
    trocar_senha   BOOLEAN NOT NULL DEFAULT false,
    ultimo_login_em TIMESTAMPTZ NULL,
    criado_em      TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE auditoria (                   -- não existe hoje; requisito novo
    id          BIGSERIAL PRIMARY KEY,
    usuario_id  BIGINT NULL REFERENCES usuario(id),
    entidade    TEXT NOT NULL,             -- 'acesso', 'usuario', 'pessoa'
    entidade_id BIGINT NULL,
    acao        TEXT NOT NULL,             -- 'CRIAR' | 'ALTERAR' | 'SAIDA' | 'LOGIN'
    dados       JSONB NULL,
    ip          TEXT NULL,
    em          TIMESTAMPTZ NOT NULL DEFAULT now()
);
```

### 6.3 Por que o snapshot

Um registro de controle de acesso é **documento histórico**. Se a pessoa muda de empresa, os acessos
do ano passado têm de continuar mostrando a empresa antiga. Guardar apenas `pessoa_id` reescreveria
o passado a cada edição de cadastro. Daí as colunas `*_snapshot` ao lado das FKs.

### 6.4 Decisões de tipo

- **`entrada_em`/`saida_em` como timestamp**, não texto. Isso corrige de uma vez o formato
  divergente (3.5) e viabiliza cálculo de permanência.
- **`prestador`/`agregado` como `BOOLEAN NULL`** — os três estados reais são sim, não e
  "não informado". Colapsar `''` e `NULL` em `false` inventaria informação.
- **`entrada_em NULL` é permitido** — é a única forma honesta de representar os 11.278 registros
  legados sem data. As telas devem exibi-los como "data desconhecida", e os filtros de período os
  excluem naturalmente.

---

## 7. Migração dos dados

Roteiro para o script de migração (executar sobre uma **cópia** de
`PORTARIA/Portaria/bin/Debug/controleAcesso.db`; conferir que `Portaria.exe` não está aberto).

```
0. Copiar o .db. Nunca migrar do arquivo em uso.

1. usuario
   SELECT * FROM USUARIO → usuario
   - login_norm = UPPER(TRIM(login))
   - senha_hash copiado literal (formato PBKDF2$ já é o da web)
   - marcar trocar_senha = true para 'admin' se a senha ainda for a padrão

2. tipo_veiculo + tipo_veiculo_alias
   SELECT DISTINCT TIPOVEICULO FROM Veiculo
   - normalizar manualmente: '¾' + '3/4' + '3/4.' → um único tipo '3/4'
   - alias de tudo que foi consolidado, para o de-para dos acessos
   - REVISÃO HUMANA OBRIGATÓRIA nesta etapa

3. empresa
   SELECT DISTINCT TRIM(EMPRESA) FROM Veiculo WHERE TRIM(IFNULL(EMPRESA,'')) <> ''
   - nome_norm = maiúsculas sem acento
   - corrigir encoding quebrado (JOSU? → JOSUÉ) — usar mapa de correção, não adivinhação
   - ~3.608 valores: NÃO tentar deduplicar por similaridade nesta fase (risco de fundir
     empresas diferentes). Migrar 1:1 e deixar a consolidação como tarefa posterior com UI.

4. pessoa (do motorista)
   Para cada documento_norm distinto em Veiculo.CPF, pegar o registro MAIS RECENTE
   (ORDER BY DataHora DESC) como fonte de nome/celular/empresa.
   - documento_norm = só dígitos e letras, maiúsculas
   - tipo_documento = 'CPF' se 11 dígitos e DV válido; 'RG' se ≤ 10 e só dígitos; senão 'OUTRO'
   - NÃO rejeitar os 48 CPFs com DV inválido: migrar com tipo_documento = 'OUTRO' e sinalizar
     numa lista de pendências para o usuário revisar

5. pessoa (dos ajudantes)
   Mesmo processo sobre CPFAJUDANTE/NOMEAJUDANTE, apenas onde CPFAJUDANTE não é vazio
   (2.722 registros). Reaproveitar a pessoa se o documento já existir.

6. veiculo
   SELECT DISTINCT PLACA → placa_norm = Placa.Normalizar(PLACA)
   - agrupar por placa_norm: 'GCT 6604' e 'GCT6604' são o MESMO veículo  << ponto crítico
   - manter em `placa` a grafia do registro mais recente
   - 'S/ PLACA' e as 93 fora de padrão: migrar como estão, marcar para revisão

7. acesso  (22.175 linhas → 22.175 linhas, 1:1, nada agregado)
   - legado_id = Veiculo.ID, origem = 'MIGRACAO_DESKTOP'
   - entrada_em = parse de DataHora ('yyyy-MM-dd HH:mm:ss'), NULL se vazio
   - saida_em   = parse de SAIDA   ('dd/MM/yyyy HH:mm:ss'), NULL se vazio
     ATENÇÃO: formato diferente de entrada. Parse com cultura pt-BR explícita.
   - prestador/agregado: 'SIM'→true, 'NAO'/'NÃO'→false, ''/NULL→NULL
   - todos os campos *_snapshot preenchidos com o valor original, sem limpeza
   - usuario_entrada_login = USUARIOENTRADA (só 2 registros têm)
   - FKs resolvidas pelos de-para das etapas 2-6

8. acesso_acompanhante
   Uma linha por acesso com CPFAJUDANTE ou NOMEAJUDANTE preenchido, ordem = 1.

9. CONTROLE: NÃO migrar. (ver 3.4)

10. Conferência (falhar o deploy se qualquer uma não bater):
    - COUNT(acesso) = 22.175
    - COUNT(acesso WHERE entrada_em IS NOT NULL) = 10.897
    - COUNT(acesso WHERE saida_em IS NOT NULL) = 9.689
    - COUNT(acesso_acompanhante) = 2.722
    - nenhum acesso sem pessoa_id
    - soma por placa_norm igual à contagem original agrupada
    - relatório de pendências: CPFs com DV inválido, placas fora de padrão, tipos consolidados
```

**Estratégia de corte:** rodar a migração num ambiente de homologação primeiro, com o desktop ainda
em produção. Validar os relatórios web contra os PDFs do desktop no mesmo período. Só então migrar
de novo (delta) e desligar o desktop. Não operar os dois em paralelo escrevendo — o SQLite em
arquivo não tolera dois donos.

---

## 8. API — endpoints

Se a escolha for SPA + API. Com Blazor Server ou Razor Pages, isto descreve os serviços de aplicação
em vez de rotas HTTP.

```
POST   /api/auth/login                 { login, senha } → { token, usuario, trocarSenha }
POST   /api/auth/logout
POST   /api/auth/trocar-senha          { senhaAtual, novaSenha }

GET    /api/acessos/hoje               ?somenteDentro=true
GET    /api/acessos/dentro             quem está na planta agora
POST   /api/acessos                    registrar entrada (com acompanhantes[])
POST   /api/acessos/{id}/saida         registrar saída
GET    /api/acessos                    ?de&ate&usuario&somenteSemSaida&page   (histórico paginado)

GET    /api/busca/placa/{placa}        última visita por placa normalizada (pré-preenchimento)
GET    /api/busca/documento/{doc}      última visita por documento normalizado
GET    /api/historico/placa/{placa}    lista de entradas/saídas
GET    /api/historico/documento/{doc}

GET    /api/pessoas                    ?q= (autocomplete por nome/documento)
GET    /api/empresas                   ?q= (autocomplete)
GET    /api/tipos-veiculo

POST   /api/relatorios/personalizado   { campos[], de, ate, usuario, somenteSemSaida }
                                       → application/pdf  (ou ?formato=xlsx|json)

GET    /api/usuarios                   [nível 1]
POST   /api/usuarios                   [nível 1]
PUT    /api/usuarios/{id}              [nível 1]
POST   /api/usuarios/{id}/senha        [nível 1]
POST   /api/usuarios/{id}/ativo        [nível 1] — bloquear se for o último admin ativo

WS     /hub/movimentacao               push da grade do dia (substitui o timer de 5s)
```

Regras de validação no servidor (nunca só no cliente):

- `nome` e `documento` obrigatórios.
- CPF (11 dígitos) com DV válido — **só para dados novos**.
- Placa: aceita 7 caracteres válidos **ou** texto livre (`S/ PLACA`). Nunca obrigatória.
- Datas: `ate ≥ de`, limite superior exclusivo.
- Lista de campos do relatório validada contra whitelist fixa no servidor (como
  `CamposDisponiveis` hoje) — **nunca** montar SQL com nome de coluna vindo do request.

---

## 9. Stack recomendada

| Camada | Escolha | Por quê |
|---|---|---|
| Backend | **ASP.NET Core 8** (LTS) | O time já é C#; as regras de `Mascaras.cs` e `Seguranca.cs` migram por cópia |
| UI | **Blazor Server** | Mantém C# ponta a ponta; push nativo por SignalR resolve a grade em tempo real sem escrever WebSocket na mão; sem build JS |
| Dados | **EF Core 8** + **PostgreSQL** | Tipos de data de verdade, `JSONB` para auditoria, índice parcial para "quem está dentro". SQL Server serve igual se já houver licença — o `Microsoft.Data.SqlClient` já está no projeto |
| PDF | **QuestPDF** (mesma lib) | Roda em ASP.NET Core; o código de layout dos dois relatórios é reaproveitável quase literalmente |
| Excel | **ClosedXML** | Já é dependência do projeto, hoje sem uso |
| Auth | ASP.NET Core Identity **ou** cookie auth próprio | Ver nota abaixo |
| Hospedagem | IIS ou Kestrel + reverse proxy, na rede interna | A portaria não precisa de internet; menos superfície de ataque |

**Nota sobre autenticação:** o formato de hash atual (`PBKDF2$50000$salt$hash`, SHA-256, 32 bytes)
não é o do ASP.NET Core Identity. Duas saídas: (a) auth própria com cookie, reaproveitando
`Usuarios.SenhaConfere()` — mais simples e preserva as 3 senhas existentes; (b) Identity com um
`IPasswordHasher` customizado que valida o formato antigo e re-hasheia no próximo login bem-sucedido.
Com 3 usuários, **(a) é suficiente**; se houver plano de SSO/AD, vale (b).

**Alternativa se a preferência for JS:** Node/NestJS ou Next.js + React + Prisma + PostgreSQL. O
modelo de dados e as regras deste documento valem sem alteração — apenas o hash PBKDF2 precisa ser
reimplementado (`crypto.pbkdf2` com os mesmos parâmetros: SHA-256, 50.000 iterações, 32 bytes).

---

## 10. Requisitos não-funcionais

- **Perfil de uso:** poucos usuários simultâneos (1-5 porteiros), ~1.500 entradas/mês, base de
  22 mil registros crescendo ~18 mil/ano. Nada aqui exige escala.
- **Latência da busca:** o porteiro digita a placa com o caminhão parado no portão. Busca por placa
  tem de responder em **< 300 ms** — daí a coluna `placa_norm` indexada em vez de normalização em
  runtime.
- **Tempo real:** grade do dia atualizada por push. O timer de 5s do desktop era polling de
  varredura completa; não replicar.
- **Disponibilidade:** a portaria opera fora do horário comercial. Se o sistema cair, o fluxo para.
  Prever (a) backup automático diário do banco, (b) plano de contingência em papel, (c) restart
  automático do serviço.
- **Rede:** intranet. Se houver guarita sem cabo, testar o Wi-Fi antes de escolher Blazor Server —
  ele mantém conexão persistente e sofre com rede instável. Rede ruim ⇒ preferir Blazor WASM ou SPA.
- **Dispositivo:** desktop no balcão hoje; prever tablet. Layout responsivo, campos e fontes grandes
  (o desktop usa Segoe UI 12 na grade por um motivo).
- **Acessibilidade de operação:** navegação por teclado completa, Enter fazendo a busca correta
  conforme o campo em foco (4.9). O porteiro não usa mouse.
- **Backup:** o desktop não tem nenhum. Requisito novo: dump diário + retenção de 30 dias + teste de
  restauração documentado.
- **LGPD:** a base tem CPF, RG, nome e celular de ~12.500 pessoas. Requisitos novos:
  acesso restrito por nível, log de auditoria (6.2), política de retenção definida com o jurídico,
  e nenhum dado pessoal em log de aplicação.

---

## 11. Fases de entrega

**Fase 0 — Fundação (1 semana)**
Projeto ASP.NET Core, EF Core, PostgreSQL, migrations, CI, login funcionando, deploy em homologação.

**Fase 1 — Paridade (3-4 semanas)**
Os 10 casos de uso de 5.1. Script de migração completo com as conferências de 7.10. Relatório PDF
validado contra os PDFs do desktop no mesmo período.
*Critério de saída:* um porteiro opera um turno inteiro na web sem recorrer ao desktop.

**Fase 2 — Corte (1 semana)**
Migração final (delta), operação assistida em paralelo por alguns dias (desktop somente leitura),
desligamento do desktop, treinamento.

**Fase 3 — Ganhos (contínuo)**
Itens de 5.2, na ordem que o usuário priorizar. Consolidação de empresas e revisão dos dados
sinalizados como pendência pela migração.

---

## 12. Riscos

| Risco | Gravidade | Mitigação |
|---|---|---|
| Consolidação de placas por `placa_norm` fundir veículos que na verdade são distintos | Alta | `placa_norm` é determinística (só remove separadores); revisar manualmente as 93 placas fora de padrão antes de migrar |
| Consolidação de 3.608 empresas por similaridade fundir empresas diferentes | Alta | **Não fazer** na migração. 1:1 agora, consolidação depois com UI e decisão humana |
| Validar CPF na leitura inviabilizar a edição dos 48 registros com DV inválido | Média | Validar só na escrita de dados novos |
| Parse de `SAIDA` com cultura errada (dd/MM vs MM/dd) corromper 9.689 datas silenciosamente | Alta | Cultura pt-BR explícita no parse + conferência de sanidade (`saida_em ≥ entrada_em` na maioria dos casos) |
| Perder registros na migração sem ninguém notar | Alta | Conferências de contagem de 7.10 como gate de deploy, não como checklist manual |
| Blazor Server em Wi-Fi instável na guarita | Média | Testar a rede na fase 0; se ruim, trocar por WASM antes de escrever a UI |
| Duas fontes de escrita (desktop + web) durante a transição | Alta | Nunca operar os dois escrevendo. Desktop em somente-leitura na fase 2 |
| `admin`/`admin` chegar à produção web | Alta | `trocar_senha = true` forçado no primeiro login; senha mínima de 8 caracteres |

---

## 13. Decisões pendentes (para o usuário)

1. **Banco:** PostgreSQL ou SQL Server? (Há licença de SQL Server disponível?)
2. **UI:** Blazor Server (mais rápido de entregar, exige rede estável) ou SPA + API (mais trabalho,
   tolera rede ruim e abre caminho para app móvel)?
3. **Agendamento** (4.6): entra na fase 1, fica para a fase 3, ou é descartado?
4. **Hospedagem:** servidor interno já existe, ou precisa ser provisionado? Acesso externo é
   necessário (portaria remota, gestor de casa)?
5. **Cadastro de pessoas** (5.2): a web deve virar cadastro consultável já na fase 1, ou manter o
   comportamento "última visita" do desktop para não mudar o hábito do porteiro?
6. **Retenção de dados** (LGPD): por quanto tempo guardar registros de acesso?
7. **Tipos de veículo** (3.5): quem revisa a lista consolidada dos ~15 tipos principais + cauda longa?

---

## 14. Anexos

### 14.1 Ambiente de build do projeto atual

O desktop **não** compila com `dotnet build` (é `packages.config`, não SDK-style):

```powershell
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' `
  'PORTARIA\Portaria.sln' /t:Rebuild /p:Configuration=Debug /v:minimal /nologo /m
```

Arquivos vindos por download trazem a marca da Web e fazem o build falhar com `MSB3821` nos `.resx`:

```powershell
Get-ChildItem -Recurse -Include *.resx,*.cs,*.csproj,*.sln,*.config | Unblock-File
```

### 14.2 Layout de pastas (atenção)

A raiz `PORTARIA - V2/` é uma **casca** com um `.git` órfão apontando para uma versão antiga do
código. O projeto real é `PORTARIA/Portaria/`, e **o código atual não está commitado em nenhum
histórico** — não contar com `git checkout` para recuperar nada. O banco de produção fica em
`PORTARIA/Portaria/bin/Debug/controleAcesso.db` (pasta de saída de build); o `controleAcesso.db` da
pasta `PORTARIA/` tem apenas dados de teste.

### 14.3 Consultas atuais, como especificação de comportamento

```sql
-- Grade do dia  (Frm_Veiculo.cs:334)
SELECT ID, CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE,
       strftime('%d/%m/%Y %H:%M', DataHora) AS ENTRADA,
       SAIDA, PLACA, TIPOVEICULO, PRESTADOR, AGREGADO, EMPRESA
FROM Veiculo
WHERE DATE(DataHora) = DATE('now')
ORDER BY DataHora DESC;

-- Busca por placa, com normalização  (Frm_Veiculo.cs:388)
SELECT ID, CPF, NOME, CELULAR, CPFAJUDANTE, NOMEAJUDANTE, DataHora, PLACA, TIPOVEICULO, EMPRESA
FROM Veiculo
WHERE REPLACE(REPLACE(REPLACE(REPLACE(UPPER(IFNULL(PLACA,'')),' ',''),'-',''),'/',''),'.','') = @PLACA
ORDER BY DataHora DESC
LIMIT 1;

-- Histórico da placa  (Frm_Veiculo.cs:505)
SELECT strftime('%d/%m/%Y %H:%M', DataHora) AS ENTRADA, SAIDA
FROM Veiculo
WHERE <placa normalizada> = $placa
ORDER BY DataHora DESC;

-- Registrar saída  (Frm_Veiculo.cs:757)
UPDATE Veiculo SET SAIDA = @saida WHERE ID = @id;   -- @saida = 'dd/MM/yyyy HH:mm:ss'

-- Relatório por período  (Frm_relatorio_personalizado.cs:299)
SELECT <campos da whitelist>
FROM VEICULO
WHERE datetime(DataHora) >= datetime(@INI)
  AND datetime(DataHora) <  datetime(@FIM)
  [AND TRIM(IFNULL(SAIDA,'')) = '']
  [AND UPPER(TRIM(IFNULL(USUARIOENTRADA,''))) = UPPER(@USU)]
ORDER BY datetime(DataHora);
```

### 14.4 Código morto — não portar

- `DataAccess.cs` — POCOs gerados por ferramenta, nunca instanciados.
- `Frm_Cadastro.cs` / `.Designer.cs` — form vazio.
- `Program.cs:45-65` — classe `conexao` aninhada, não usada.
- `Frm_Veiculo.cs:784-847` (`Btn_CriarID_Click_Click`) — migração manual de schema exposta como
  botão na tela do porteiro. Faz `DROP TABLE Veiculo`. **Jamais** replicar na web.
- `Banco.cs` inteiro — migração automática de schema em runtime. Na web isso é papel do EF Core
  Migrations, executado no deploy, não na abertura do app.
- Dezenas de event handlers vazios nos forms (`label1_Click`, `textBox4_TextChanged`, …).
- `Microsoft.Data.SqlClient` e `ClosedXML` como dependências sem uso.

### 14.5 Constantes de negócio

```
Nível de acesso:        1 = total (inclui cadastro de usuários)
                        2 = restrito (tudo menos cadastro de usuários)
Hash de senha:          PBKDF2-SHA256, 50.000 iterações, salt 16 bytes, hash 32 bytes
                        formato: PBKDF2$<iter>$<salt b64>$<hash b64>
Senha mínima (desktop): 4 caracteres    → usar 8+ na web
Usuário inicial:        admin / admin, nível 1
Placa:                  7 caracteres, LLL9999 (antiga) ou LLL9L99 (Mercosul)
CPF:                    11 dígitos, DV módulo 11, rejeita dígitos repetidos
Formato de entrada:     yyyy-MM-dd HH:mm:ss
Formato de saída:       dd/MM/yyyy HH:mm:ss    (divergente — corrigir)
Timer da grade:         5.000 ms (desktop)     → substituir por push
Prestador (opções):     SIM / NAO
Agregado (opções):      SIM / NÃO              (com til — divergente de Prestador)
Cor de campo inválido:  #FFCDCD
```
