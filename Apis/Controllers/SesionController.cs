using Application.Interfaces;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Apis.Controllers
{

    [Route("Api/Sesion")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ISesionService _sesionService;

        public SesionController(ISesionService sesionService)
        {
            _sesionService = sesionService;
        }



        [HttpPost("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] ReqIniciarSesion request)
        {
            ResBase response = await _sesionService.IniciarSesionAsync(request);

            if (response.resultado)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

















    }
}
