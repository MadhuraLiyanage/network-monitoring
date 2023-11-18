using Microsoft.AspNetCore.Mvc;
using System.Net;
using TPOSMonitoring.Models;
using TPOSMonitoring.ServerApi.Enum;
using TPOSMonitoring.ServerApi.Models;
using TPOSMonitoring.ServerApi.Services;


namespace TPOSMonitoring.ServerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TPOSMonitoringServerAPI : ControllerBase
    {
        private readonly ITPOSMonitoringRepository _tPOSMonitoringRepository = new TPOSMonitoringRepository();
        private readonly CheckStatus _checkStatus = new CheckStatus();

        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<IEnumerable<ServiceStatus>>> GetRemortHostStatus([FromHeader] string token)
        {
            TextLogger.LogToText(Enum.LoogerType.Information, "Request received to GetRemortHostStatus");
            //validate API token
            if (!GlobalStaticVariables.Token.Equals(token))
            {
                TextLogger.LogToText(LoogerType.Error, "Invalid Token received. Token  " + token);
                return BadRequest("Invalid token");
            }

            TextLogger.LogToText(LoogerType.Information, "Token validated successfuly");

            
            try
            {
                IEnumerable<ServiceStatus> serviceStatuses = await _checkStatus.ProcessParallel();
                return Ok(serviceStatuses);
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, "Error getting status from other locations. Error : " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting status from other locations. Exception : " + ex.Message);

            }
        }
    }
}
