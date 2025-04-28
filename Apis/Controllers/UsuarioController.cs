using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace Apis.Controllers
{
    [Route("Api/Usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario([FromBody] ReqCrearUsuario request)
        {
            // Llamar al método de negocio
            ResBase res = await _usuarioService.CrearUsuarioAsync(request);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Usuarios/CrearUsuario");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }
                return BadRequest(res.detalle);
            }
        }
    }













       
}
