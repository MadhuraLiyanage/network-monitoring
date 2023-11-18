using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace TPOSMonitoring.PosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
    public class APIHealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _service;
        public APIHealthCheckController(HealthCheckService service)
        {
            _service = service;
        }

        [HttpHead]
        public async Task<IActionResult> Get()
        {
            var report = await _service.CheckHealthAsync();

            return report.Status == HealthStatus.Healthy ? Ok() : StatusCode((int)HttpStatusCode.ServiceUnavailable);
            //return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
        }
    }
}
