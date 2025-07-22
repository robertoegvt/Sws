using Newtonsoft.Json.Linq;

namespace Sws.Models
{
    public static class Funcoes
    {
        public static JToken RetornarErro(CodigoErro codigo, string message = null)
        {
            return JToken.FromObject(new { Codigo = codigo.ToString(), Mensagem = message });
        }
    }

    public enum CodigoErro
    {
        MHOB000,
        MHOB002,
        MHOB003,
        MHOB004,
        MHOB005,
        MHOB006,
        MHOB007
    }
}
