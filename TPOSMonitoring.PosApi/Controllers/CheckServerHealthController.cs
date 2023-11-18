using Microsoft.AspNetCore.Mvc;
using System.Management;
using System.Net;
using System.ServiceProcess;
using TPOSMonitoring.Models;

namespace TPOSMonitoring.PosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckServerHealthController : ControllerBase
    {
        private readonly static ServiceController sc = new ServiceController();
        //private static ServiceStatus serviceStatus = new ServiceStatus();
        private static PosServiceStatus posServiceStatus = new PosServiceStatus();


        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0, Location = ResponseCacheLocation.None)]
        public async Task<ActionResult<PosServiceStatus>> GetServiceHealth([FromHeader] string token)
        {
            //validate API tocken
            if (!GlobalStaticVariables.Token.Equals(token))
            {
                return BadRequest("Invalid token");
            }

            try
            {
                ///Host name
                //serviceStatus.RemortHostName = Dns.GetHostEntry(HttpContext.Connection.RemoteIpAddress).HostName; //.Split('.')[0];
                ///Sql Server Service Status
                //sc.ServiceName = GlobalStaticVariables.SqlServerServiceName;
                //serviceStatus.SqlServerStatus = sc.Status.ToString();
                //serviceStatus.SqlServerStartupType = GetStartupType(GlobalStaticVariables.SqlServerServiceName);

                ///Sql Server Agent Service Status
                //sc.ServiceName = GlobalStaticVariables.SqlServerAgentServiceName;
                //serviceStatus.SqlServerAgnetStatus = sc.Status.ToString();
                //serviceStatus.SqlServerAgnetStartupType = GetStartupType(GlobalStaticVariables.SqlServerAgentServiceName);


                //Sql Server Service Status
                sc.ServiceName = GlobalStaticVariables.SqlServerServiceName;
                posServiceStatus.SqlServerStatus = sc.Status.ToString();
                posServiceStatus.SqlServerStartupType = GetStartupType(GlobalStaticVariables.SqlServerServiceName);

                //Sql Server Agent Service Status
                sc.ServiceName = GlobalStaticVariables.SqlServerAgentServiceName;
                posServiceStatus.SqlServerAgnetStatus = sc.Status.ToString();
                posServiceStatus.SqlServerAgnetStartupType = GetStartupType(GlobalStaticVariables.SqlServerAgentServiceName);



                return Ok(posServiceStatus);
            }
            catch
            {
                posServiceStatus.SqlServerStatus = "N/A";
                posServiceStatus.SqlServerStartupType = "N/A";
                posServiceStatus.SqlServerAgnetStatus = "N/A";
                posServiceStatus.SqlServerAgnetStartupType = "N/A";
                return Ok(posServiceStatus);
            }
        }

        private string GetStartupType(string svcName)
        {
            string startMode = "";
            string filter = String.Format("SELECT StartMode FROM Win32_Service WHERE Name = '{0}'", svcName);
            ManagementObjectSearcher svc = new ManagementObjectSearcher(filter);

            if (svc == null)
            {
                return "";
            }
            else
            {
                try
                {
                    using (ManagementObjectCollection services = svc.Get())
                    {
                        foreach (ManagementObject service in services)
                        {
                            startMode = service.GetPropertyValue("StartMode").ToString();
                        }
                    }
                }
                catch
                {
                    startMode = "";
                }
            }
            return startMode;
        }
    }
}
