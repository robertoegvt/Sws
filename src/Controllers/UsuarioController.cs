using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Sws.Extensions;

namespace Sws.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly SysContext _context;

        public UsuarioController(SysContext context)
        {
            _context = context;
        }

        [HttpGet("AutenticarUsuario")]
        public async Task<IActionResult> AutenticarUsuario(string usuarioWS, string senhaWS, string usuario, string senha, string origem = "MHOB ME")
        {
            try
            {
                JToken result = await _context.AutenticarUsuarioAsync(usuarioWS, senhaWS, usuario, senha, origem);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return this.RetornarErro(CodigoErro.MHOB000, ex.Message);
            }
        }
    }
}
