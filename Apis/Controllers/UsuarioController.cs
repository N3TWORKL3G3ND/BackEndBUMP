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
                return BadRequest(new
                {
                    res.detalle
                });
            }
        }



        [HttpPost("ValidarCorreo")]
        public async Task<IActionResult> ValidarCorreo([FromBody] ReqValidarCorreo request)
        {
            // Llamar al método de negocio
            ResBase res = await _usuarioService.ValidarCorreoAsync(request);

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
                Console.WriteLine("\nApi/Usuarios/ValidarCorreo");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }
                return BadRequest(new
                {
                    res.detalle
                });
            }
        }



        [HttpPost("GenerarCodigoVerificacion")]
        public async Task<IActionResult> GenerarCodigoVerificacion([FromBody] ReqGenerarCodigo request)
        {
            ResBase res = await _usuarioService.GenerarCodigoVerificacionAsync(request);

            if (res.resultado)
            {
                return Ok(new { res.detalle });
            }
            else
            {
                Console.WriteLine("\nApi/Usuarios/GenerarCodigoVerificacion");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }

                return BadRequest(new { res.detalle });
            }
        }






































    }
}
