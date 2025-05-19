using Application.Interfaces;
using Application.Services;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apis.Controllers
{
    [Route("Api/Seguimiento")]
    [ApiController]
    [Authorize]
    public class SeguimientoController : ControllerBase
    {
        private readonly ISeguimientoService _seguimientoService;

        public SeguimientoController(ISeguimientoService seguimientoService)
        {
            _seguimientoService = seguimientoService;
        }



        [HttpGet("ListarSintomasCatalogo")]
        public async Task<IActionResult> ListarSintomasCatalogo()
        {
            // Llamar al método de negocio
            var res = await _seguimientoService.ListarSintomasCatalogoAsync();

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaSintomas
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Seguimiento/SintomasCatalogo");
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



        [HttpPost("RegistrarSintoma")]
        public async Task<IActionResult> RegistrarSintoma([FromBody] ReqRegistrarSintoma request)
        {
            // Llamar al método de negocio
            ResBase res = await _seguimientoService.RegistrarSintomaAsync(request, User);

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
                Console.WriteLine("\nApi/Seguimiento/RegistrarSintoma");
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



        [HttpPost("RegistrarEventualidad")]
        public async Task<IActionResult> RegistrarEventualidad([FromBody] ReqRegistrarEventualidad request)
        {
            // Llamar al método de negocio
            ResBase res = await _seguimientoService.RegistrarEventualidadAsync(request, User);

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
                Console.WriteLine("\nApi/Seguimiento/RegistrarEventualidad");
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



        [HttpPost("RegistrarContraccion")]
        public async Task<IActionResult> RegistrarContraccion([FromBody] ReqRegistrarContraccion request)
        {
            // Llamar al método de negocio
            ResBase res = await _seguimientoService.RegistrarContraccionAsync(request, User);

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
                Console.WriteLine("\nApi/Seguimiento/RegistrarContraccion");
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



















    }
}
