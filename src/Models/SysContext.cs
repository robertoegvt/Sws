using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Sws.Models
{
    public class SysContext
    {
        public HttpContext HttpContext { get; set; }

        public Task<bool> ValidarUsuarioWS(string usuario, string senha) => Task.FromResult(true);
        public Task<SenhaPolitica> ObtemSenhaPolitica() => Task.FromResult(new SenhaPolitica());
        public Task<DadosUsuario> ValidarUsuario(string usuario, string senha, bool logonCaseSentisitive, string origem) => Task.FromResult(new DadosUsuario());
        public Task<Vendedor> ObtemVendedor(int chave) => Task.FromResult<Vendedor>(null);
        public Task<Escopo> PopulaTEscopoVendedor(int chaveUsuario) => Task.FromResult(new Escopo());
        public Task<Licenca> ObtemLicenca() => Task.FromResult(new Licenca());
        public Task<TarefaCor> ObtemTarefaCorDafault() => Task.FromResult<TarefaCor>(null);
        public Task<DadosPadroes> ObterDadosPadroes(int chave) => Task.FromResult(new DadosPadroes());
        public Task<Job> ObtemJob(int chave) => Task.FromResult<Job>(null);
        public Task<string> FGlobal(string nomeParametro) => Task.FromResult(string.Empty);
        public Task<Moeda> ObtemMoeda(int chaveMoeda) => Task.FromResult<Moeda>(null);
        public Task<string> PAnalisaDireito(int chaveUsuario, string direito) => Task.FromResult(string.Empty);
        public string CriarToken(int chaveUsuario) => string.Empty;
    }
}
