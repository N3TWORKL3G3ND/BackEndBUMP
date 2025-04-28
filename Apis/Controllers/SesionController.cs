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


        

















    }
}
