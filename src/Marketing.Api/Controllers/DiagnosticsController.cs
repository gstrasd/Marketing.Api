using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Marketing.Api.Controllers
{
    [Route("marketing")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private readonly IHostEnvironment _environment;

        public DiagnosticsController(IHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public object GetDiagnostics()
        {
            return new
            {
                Application = _environment.ApplicationName,
                Assembly.GetExecutingAssembly().GetName().Version,
                Environment = _environment.EnvironmentName,
            };
        }
    }
}
