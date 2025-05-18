using Application.Interfaces;
using Application.Services;
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



























    }
}
