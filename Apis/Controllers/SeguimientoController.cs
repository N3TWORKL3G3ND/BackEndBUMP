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



        [HttpGet("ListarContracciones")]
        public async Task<IActionResult> ListarContracciones()
        {
            // Llamar al método de negocio
            var res = await _seguimientoService.ListarContraccionesAsync(User);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaContracciones
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Seguimiento/ListarContracciones");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }
                return BadRequest(new
                {
                    res.detalle,
                    res.errores
                });
            }
        }



        [HttpGet("ListarEventualidades")]
        public async Task<IActionResult> ListarEventualidades()
        {
            // Llamar al método de negocio
            var res = await _seguimientoService.ListarEventualidadesAsync(User);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaEventualidades
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Seguimiento/ListarEventualidades");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }
                return BadRequest(new
                {
                    res.detalle,
                    res.errores
                });
            }
        }



        [HttpGet("ListarRegistroSintomas")]
        public async Task<IActionResult> ListarRegistroSintomas()
        {
            // Llamar al método de negocio
            var res = await _seguimientoService.ListarRegistroSintomasAsync(User);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaRegistroSintomas
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Seguimiento/ListarRegistroSintomas");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }
                return BadRequest(new
                {
                    res.detalle,
                    res.errores
                });
            }
        }






























    }
}
