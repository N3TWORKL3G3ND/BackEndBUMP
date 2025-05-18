using Application.Interfaces;
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































    }
}
