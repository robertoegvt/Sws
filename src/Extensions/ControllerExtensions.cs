using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Sws.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult RetornarErro(this ControllerBase controller, CodigoErro codigo, string message)
        {
            var json = Funcoes.RetornarErro(codigo, message);
            return controller.StatusCode(500, json);
        }
    }
}
