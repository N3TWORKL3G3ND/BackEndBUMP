using Application.Interfaces;
using Application.Services;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Apis.Controllers
{

    [Route("Api/Sesion")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ISesionService _sesionService;
        private readonly IConfiguration _configuration;

        public SesionController(ISesionService sesionService, IConfiguration configuration)
        {
            _sesionService = sesionService;
            _configuration = configuration;
        }



        [HttpGet("test-db")]
        public IActionResult TestDbConnection()
        {
            try
            {
                using var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                conn.Open();
                return Ok("¡Conexión exitosa!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] ReqLoginUsuario request)
        {
            // Llamar al método de negocio
            ResLoginUsuario res = await _sesionService.LoginUsuarioAsync(request);

            if (res.resultado)
            {
                return Ok(new
                {
                    res.detalle,
                    res.correoVerificado,
                    res.token
                });
            }
            else
            {
                // Log de errores en consola
                Console.WriteLine("\nApi/Sesion/Login");
                foreach (var error in res.errores)
                {
                    Console.WriteLine(error);
                }
                return BadRequest(new
                {
                    res.detalle,
                    res.correoVerificado,
                    res.token
                });
            }
        }



        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] ReqLoginUsuario request)
        {
            // Llamar al método de negocio
            ResBase res = await _sesionService.CerrarSesionAsync();

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
                Console.WriteLine("\nApi/Sesion/Logout");
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
