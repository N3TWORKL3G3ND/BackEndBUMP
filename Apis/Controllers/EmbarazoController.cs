using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apis.Controllers
{
    [Route("Api/Embarazo")]
    [ApiController]
    [Authorize]
    public class EmbarazoController : ControllerBase
    {
        private readonly IEmbarazoService _embarazoService;

        public EmbarazoController(IEmbarazoService embarazoService)
        {
            _embarazoService = embarazoService;
        }



        [HttpPost("RegistrarEmbarazo")]
        public async Task<IActionResult> RegistrarEmbarazo([FromBody] ReqRegistrarEmbarazo request)
        {
            // Llamar al método de negocio
            ResBase res = await _embarazoService.RegistrarEmbarazoAsync(request, User);

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
                Console.WriteLine("\nApi/Embarazo/RegistrarEmbarazo");
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



        [HttpGet("ListarEmbarazos")]
        public async Task<IActionResult> ListarEmbarazos()
        {
            // Llamar al método de negocio
            var res = await _embarazoService.ListarEmbarazosAsync(User);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaEmbarazos
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Embarazo/ListarEmbarazos");
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



        [HttpPost("RegistrarProgresoEmbarazo")]
        public async Task<IActionResult> RegistrarProgresoEmbarazo([FromBody] ReqRegistrarProgresoEmbarazo request)
        {
            // Llamar al método de negocio
            ResBase res = await _embarazoService.RegistrarProgresoEmbarazoAsync(request);

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
                Console.WriteLine("\nApi/Embarazo/RegistrarProgresoEmbarazo");
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



        [HttpGet("ListarProgresosEmbarazo/{idEmbarazo}")]
        public async Task<IActionResult> ListarProgresosEmbarazo(int idEmbarazo)
        {
            // Llamar al método de negocio
            var res = await _embarazoService.ListarProgresosEmbarazoAsync(idEmbarazo);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaProgresos
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Embarazo/ListarProgresosEmbarazo");
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



        [HttpGet("ListarHospitales")]
        public async Task<IActionResult> ListarHospitales()
        {
            // Llamar al método de negocio
            var res = await _embarazoService.ListarHospitalesAsync();

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaHospitales
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Hospital/ListarHospitales");
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



        [HttpPost("RegistrarCita")]
        public async Task<IActionResult> RegistrarCita([FromBody] ReqRegistrarCita request)
        {
            // Llamar al método de negocio
            ResBase res = await _embarazoService.RegistrarCitaAsync(request, User);

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
                Console.WriteLine("\nApi/Cita/RegistrarCita");
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



        [HttpGet("ListarCitas")]
        public async Task<IActionResult> ListarCitas()
        {
            // Llamar al método de negocio
            var res = await _embarazoService.ListarCitasAsync(User);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.listaCitas
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Cita/ListarCitas");
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
