using System;

namespace Sws.Models
{
    public class DadosUsuario
    {
        public int Chave { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public bool SYS_MOBILE { get; set; }
        public bool SYS_MHOB_CRM { get; set; }
        public bool SYS_APS_APONTAMENTOS { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string Empresa { get; set; } = string.Empty;
        public string LicencaCnpj { get; set; } = string.Empty;
        public int? ChaveJob { get; set; }
        public DateTime DataAlteracaoSenha { get; set; } = DateTime.Now;
        public string UrlFoto { get; set; } = string.Empty;
        public string Erro { get; set; } = string.Empty;
        public string MHOBME_CLIENTES_INFORMACOES { get; set; }
        public string MHOBME_FATURAMENTO { get; set; }
        public string MHOBME_BSC { get; set; }
        public string MHOBME_CLIENTES { get; set; }
        public string MHOBME_PRODUTOS { get; set; }
        public string MHOBME_FINANCEIRO { get; set; }
        public string MHOBME_PROCESSOS { get; set; }
        public string MHOBME_APROVACAO_VENDA { get; set; }
        public string MHOBME_APROVACAO_COMPRA { get; set; }
        public string MHOBME_ANALYSIS_MAIL { get; set; }
        public string MHOBME_ANALISE_CONSOLIDADA { get; set; }
        public string MHOBME_EFICIENCIA_ORC { get; set; }
        public string AUTORIZAR_PAGAMENTOS { get; set; }
        public string MHOBCRM_CLIENTES_INFORMACOES { get; set; }
        public string MHOBCRM_ANALYSIS_MAIL { get; set; }
        public string MHOBCRM_CLIENTES { get; set; }
        public string MHOBCRM_PRODUTOS { get; set; }
        public string MHOBCRM_FATURAMENTO { get; set; }
        public string MHOBCRM_ACESSAR_VENDAS { get; set; }
        public string MHOBCRM_REALIZAR_VENDAS { get; set; }
        public string MHOBCRM_METAS { get; set; }
        public string MHOBCRM_OPORTUNIDADES { get; set; }
        public string MHOBCRM_ANALISE_CONSOLIDADA { get; set; }
        public string MHOBCRM_EFICIENCIA_ORC { get; set; }
        public string MHOBCRM_FATURAR_LIQUIDA_ORC { get; set; }
        public string CONSULTAS_PEDIDO { get; set; }
        public string CADASTROS_CONTATOS { get; set; }
        public string COR_PADRAO_TAREFA { get; set; }
        public bool VENDEDOR { get; set; }
        public bool VENDEDOR_RESPONSAVEL { get; set; }
        public bool FILTRAR_MATRIZ_VENDEDORES { get; set; }
        public bool FORMACAO_PRECOS { get; set; }
        public bool LIBERAR_CREDITO { get; set; }
        public bool POSICAO_FINANCEIRA { get; set; }
        public string CONTRATOS_FORNECIMENTO { get; set; }
        public bool ORCAMENTOX { get; set; }
        public bool BIBLIOTECA_CLIENTES { get; set; }
        public bool BIBLIOTECA_PRODUTO { get; set; }
        public bool ALTERA_BONIFICACAO { get; set; }
        public bool AlterarOrcamentosOwner { get; set; }
        public int? CHAVE_VENDEDOR { get; set; }
        public string Senha { get; set; }
    }
}
