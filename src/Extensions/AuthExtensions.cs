using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Sws.Models;

namespace Sws.Extensions
{
    public static class AuthExtensions
    {
        public static async Task<JToken> AutenticarUsuarioAsync(this SysContext context, string usuarioWS, string senhaWS, string usuario, string senha, string origem = "MHOB ME")
        {
            JToken json = null;

            try
            {
                if (!await context.ValidarUsuarioWS(usuarioWS, senhaWS))
                {
                    return Funcoes.RetornarErro(CodigoErro.MHOB002);
                }

                SenhaPolitica senhaPolitica = await context.ObtemSenhaPolitica();
                DadosUsuario objRetorno = await context.ValidarUsuario(usuario, senha, senhaPolitica.LogonCaseSentisitive, origem);

                if (objRetorno == null)
                {
                    return Funcoes.RetornarErro(CodigoErro.MHOB003);
                }

                if (origem == "MHOB ME")
                {
                    if (objRetorno.SYS_MOBILE || objRetorno.SYS_MHOB_CRM || objRetorno.SYS_APS_APONTAMENTOS)
                    {
                        return Funcoes.RetornarErro(CodigoErro.MHOB006);
                    }
                }

                if (origem == "MHOB CRM")
                {
                    if (objRetorno.SYS_MOBILE || objRetorno.SYS_APS_APONTAMENTOS)
                    {
                        return Funcoes.RetornarErro(CodigoErro.MHOB006);
                    }

                    if (objRetorno.SYS_MHOB_CRM)
                    {
                        if (!objRetorno.VENDEDOR || objRetorno.VENDEDOR_RESPONSAVEL || objRetorno.FILTRAR_MATRIZ_VENDEDORES)
                        {
                            return Funcoes.RetornarErro(CodigoErro.MHOB007);
                        }
                    }
                }

                objRetorno.Senha = senha;

                if (NecessitaMudarSenhaUsuario(objRetorno, senhaPolitica))
                    return json;

                if (!string.IsNullOrEmpty(objRetorno.UrlFoto))
                {
                    HttpContext http = context.HttpContext;
                    objRetorno.UrlFoto = string.Format("{0}://{1}:{2}{3}/{4}", http.Request.Scheme, http.Request.Host.Host, http.Request.Host.Port, http.Request.PathBase, objRetorno.UrlFoto);
                }

                Vendedor vendedor = null;

                if (objRetorno.VENDEDOR && objRetorno.CHAVE_VENDEDOR != null)
                    vendedor = await context.ObtemVendedor(objRetorno.CHAVE_VENDEDOR.Value);

                Escopo escopo = await context.PopulaTEscopoVendedor(objRetorno.Chave);

                Licenca licenca = await context.ObtemLicenca();
                string releaseAtual = licenca.ReleaseMaximo;

                if (string.IsNullOrEmpty(objRetorno.COR_PADRAO_TAREFA))
                {
                    TarefaCor tarefaCor = await context.ObtemTarefaCorDafault();
                    objRetorno.COR_PADRAO_TAREFA = tarefaCor == null ? string.Empty : tarefaCor.Chave.ToString();
                }

                DadosPadroes dadosPadroes = await context.ObterDadosPadroes(objRetorno.Chave);

                if (dadosPadroes.Job != null)
                    dadosPadroes.Job = await context.ObtemJob(dadosPadroes.Job.Chave);

                int.TryParse(await context.FGlobal("HYZ316505_2"), out int hyz316505_2);
                Moeda moedaPadraoOportunidades = await context.ObtemMoeda(hyz316505_2);

                bool faturarPedido = true;

                if (await context.PAnalisaDireito(objRetorno.Chave, "FATURAMENTO") != "LIBERADO")
                    faturarPedido = false;
                else if (objRetorno.CONSULTAS_PEDIDO == "SIM")
                {
                    if (await context.PAnalisaDireito(objRetorno.Chave, "PEDIDOS") == "NAO")
                        faturarPedido = false;
                }
                else if (await context.FGlobal("HYZ254385") == "SIM")
                {
                    if (await context.PAnalisaDireito(objRetorno.Chave, "PERMITIR_FATURAR_PEDIDO") != "LIBERADO")
                        faturarPedido = false;
                }

                json = JToken.FromObject(new
                {
                    Chave = objRetorno.Chave,
                    Erro = objRetorno.Erro,
                    Token = context.CriarToken(objRetorno.Chave),
                    Nome = objRetorno.Nome,
                    Sexo = objRetorno.Sexo,
                    UrlFoto = objRetorno.UrlFoto?.Replace("\\u0026", "&"),
                    Cargo = objRetorno.Cargo,
                    Licenca = objRetorno.Empresa,
                    LicencaCnpj = objRetorno.LicencaCnpj,
                    ReleaseAtual = releaseAtual,
                    BaseConectada = licenca.BaseConectada,
                    MHOBME_CLIENTES_INFORMACOES = objRetorno.MHOBME_CLIENTES_INFORMACOES,
                    MHOBME_FATURAMENTO = objRetorno.MHOBME_FATURAMENTO,
                    MHOBME_BSC = objRetorno.MHOBME_BSC,
                    MHOBME_CLIENTES = objRetorno.MHOBME_CLIENTES,
                    MHOBME_PRODUTOS = objRetorno.MHOBME_PRODUTOS,
                    MHOBME_FINANCEIRO = objRetorno.MHOBME_FINANCEIRO,
                    MHOBME_PROCESSOS = objRetorno.MHOBME_PROCESSOS,
                    MHOBME_APROVACAO_VENDA = objRetorno.MHOBME_APROVACAO_VENDA,
                    MHOBME_APROVACAO_COMPRA = objRetorno.MHOBME_APROVACAO_COMPRA,
                    MHOBME_ANALYSIS_MAIL = objRetorno.MHOBME_ANALYSIS_MAIL,
                    MHOBME_ANALISE_CONSOLIDADA = objRetorno.MHOBME_ANALISE_CONSOLIDADA,
                    MHOBME_EFICIENCIA_ORC = objRetorno.MHOBME_EFICIENCIA_ORC,
                    AUTORIZAR_PAGAMENTOS = objRetorno.AUTORIZAR_PAGAMENTOS,
                    MHOBCRM_CLIENTES_INFORMACOES = objRetorno.MHOBCRM_CLIENTES_INFORMACOES,
                    MHOBCRM_ANALYSIS_MAIL = objRetorno.MHOBCRM_ANALYSIS_MAIL,
                    MHOBCRM_CLIENTES = objRetorno.MHOBCRM_CLIENTES,
                    MHOBCRM_PRODUTOS = objRetorno.MHOBCRM_PRODUTOS,
                    MHOBCRM_FATURAMENTO = objRetorno.MHOBCRM_FATURAMENTO,
                    MHOBCRM_ACESSAR_VENDAS = objRetorno.MHOBCRM_ACESSAR_VENDAS,
                    MHOBCRM_REALIZAR_VENDAS = objRetorno.MHOBCRM_REALIZAR_VENDAS,
                    MHOBCRM_METAS = objRetorno.MHOBCRM_METAS,
                    MHOBCRM_OPORTUNIDADES = objRetorno.MHOBCRM_OPORTUNIDADES,
                    MHOBCRM_ANALISE_CONSOLIDADA = objRetorno.MHOBCRM_ANALISE_CONSOLIDADA,
                    MHOBCRM_EFICIENCIA_ORC = objRetorno.MHOBCRM_EFICIENCIA_ORC,
                    MHOBCRM_FATURAR_LIQUIDA_ORC = objRetorno.MHOBCRM_FATURAR_LIQUIDA_ORC,
                    CONSULTAS_PEDIDO = objRetorno.CONSULTAS_PEDIDO,
                    CADASTROS_CONTATOS = objRetorno.CADASTROS_CONTATOS,
                    COR_PADRAO_TAREFA = objRetorno.COR_PADRAO_TAREFA,
                    FORMACAO_PRECOS = objRetorno.FORMACAO_PRECOS,
                    LIBERAR_CREDITO = objRetorno.LIBERAR_CREDITO,
                    POSICAO_FINANCEIRA = objRetorno.POSICAO_FINANCEIRA,
                    CONTRATOS_FORNECIMENTO = objRetorno.CONTRATOS_FORNECIMENTO,
                    ORCAMENTOX = objRetorno.ORCAMENTOX,
                    BIBLIOTECA_CLIENTES = objRetorno.BIBLIOTECA_CLIENTES,
                    BIBLIOTECA_PRODUTO = objRetorno.BIBLIOTECA_PRODUTO,
                    ALTERA_BONIFICACAO = objRetorno.ALTERA_BONIFICACAO,
                    FaturarPedido = faturarPedido,
                    UsuarioVendedor = objRetorno.VENDEDOR,
                    AlterarOrcamentosOwner = objRetorno.AlterarOrcamentosOwner,
                    Vendedor = vendedor == null ? null : JToken.FromObject(new
                    {
                        vendedor.Chave,
                        vendedor.Nome,
                        vendedor.Nomered,
                        vendedor.CnpjCpf,
                        vendedor.Ativo
                    }),
                    Escopo = escopo.StringEscopo,
                    DadosPadroes = JToken.FromObject(new
                    {
                        Job = dadosPadroes.Job == null ? null : JToken.FromObject(new
                        {
                            dadosPadroes.Job.Chave,
                            dadosPadroes.Job.Nome,
                            dadosPadroes.Job.Descricao,
                            dadosPadroes.Job.Cnpj
                        }),
                        Moeda = dadosPadroes.Moeda == null ? null : JToken.FromObject(new
                        {
                            dadosPadroes.Moeda.Chave,
                            dadosPadroes.Moeda.Descricao,
                            dadosPadroes.Moeda.Sigla
                        }),
                        MoedaPadraoParaOportunidades = moedaPadraoOportunidades == null ? null : JToken.FromObject(new
                        {
                            moedaPadraoOportunidades.Chave,
                            moedaPadraoOportunidades.Descricao,
                            moedaPadraoOportunidades.Sigla
                        }),
                        PedidoTipo = dadosPadroes.PedidoTipo == null ? null : JToken.FromObject(new
                        {
                            dadosPadroes.PedidoTipo.Chave,
                            dadosPadroes.PedidoTipo.Descricao,
                            dadosPadroes.PedidoTipo.TipoVenda
                        })
                    })
                });
            }
            catch (Exception ex)
            {
                json = Funcoes.RetornarErro(CodigoErro.MHOB000, ex.Message);
            }

            return json;
        }

        private static bool NecessitaMudarSenhaUsuario(DadosUsuario usuario, SenhaPolitica senhaPolitica)
        {
            bool necessitaMudarSenha = false;
            int xValidadeSenha = 0;

            if (senhaPolitica.ImporValidade)
                xValidadeSenha = senhaPolitica.ImporValidadeDias;

            if (xValidadeSenha > 0)
            {
                DateTime dataAtual = DateTime.Today;
                if (dataAtual > usuario.DataAlteracaoSenha.AddDays(xValidadeSenha))
                {
                    Funcoes.RetornarErro(CodigoErro.MHOB004);
                    necessitaMudarSenha = true;
                }
            }

            if (usuario.Senha == senhaPolitica.SenhaPadraoNovos)
            {
                Funcoes.RetornarErro(CodigoErro.MHOB005);
                necessitaMudarSenha = true;
            }

            return necessitaMudarSenha;
        }
    }
}
