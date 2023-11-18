using Newtonsoft.Json;
using Serilog;
using TPOSMonitoring.Models;
using TPOSMonitoring.ServerApi.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using TPOSMonitoring.ServerApi.Services;

namespace TPOSMonitoring.ServerApi.Models
{
    public class CheckStatus
    {
        private readonly ITPOSMonitoringRepository _tPOSMonitoringRepository = new TPOSMonitoringRepository();
        private HttpClient _httpClient = new();
        //private List<ServiceStatus> ServiceStatuses = new(); // List<ServiceStatus>();

        public async Task<IEnumerable<ServiceStatus>> ProcessParallel()
        {
            IEnumerable<RemorteHostName> hostNames = _tPOSMonitoringRepository.GetRemortHostNames();

            TextLogger.LogToText(LoogerType.Information, $"Remorte host names extracted ({hostNames.Count()})");
            var source = new CancellationTokenSource(TimeSpan.FromSeconds(120));
            List<ServiceStatus> serviceStatuses = new();
            ServiceStatus serviceStatus = null;

            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 100,
                CancellationToken = source.Token

            };

            await Parallel.ForEachAsync(hostNames, parallelOptions, async (req, _) =>
            {
                serviceStatus = await GetRemorteHostStatus(req);
                serviceStatuses.Add(serviceStatus);
            });

            return serviceStatuses;
        }




        private async Task<ServiceStatus> GetRemorteHostStatus(RemorteHostName remortHostName)
        {
            ServiceStatus serviceStatus = new ServiceStatus();

            try
            {
                bool pingResult = await PingWithHttpClient(GlobalStaticVariables.RemorteHostStatusUri.Replace("<RemorteHostName>", remortHostName.RemortTPOSHostName) + GlobalStaticVariables.APIHealthCheckEndpoint);

                if (pingResult)
                {
                    using (HttpClient _httpClient = new())
                    {
                        try
                        {
                            _httpClient.BaseAddress = new Uri(GlobalStaticVariables.RemorteHostStatusUri.Replace("<RemorteHostName>", remortHostName.RemortTPOSHostName));
                            _httpClient.DefaultRequestHeaders.Clear();
                            _httpClient.DefaultRequestHeaders.Add("token", GlobalStaticVariables.Token);

                            var result = await _httpClient.GetAsync(GlobalStaticVariables.RemortHostStatusEndpint);
                            if (result.IsSuccessStatusCode)
                            {
                                var json = await result.Content.ReadAsStringAsync();
                                serviceStatus = JsonConvert.DeserializeObject<ServiceStatus>(json);
                                serviceStatus.RemortHostName = remortHostName.RemortTPOSHostName;
                                serviceStatus.RemortHostType = remortHostName.RemortHostType;
                                serviceStatus.RemortHostDescription = remortHostName.RemortHostDescription;
                                serviceStatus.Parent = remortHostName.Parent;

                                if (serviceStatus.SqlServerStatus == "Running" && serviceStatus.SqlServerAgnetStatus == "Running")
                                {
                                    serviceStatus.IsRunning = 1;
                                }
                                else
                                {
                                    serviceStatus.IsRunning = 0;
                                }

                            }
                            else
                            {
                                serviceStatus.RemortHostName = remortHostName.RemortTPOSHostName;
                                serviceStatus.RemortHostType = remortHostName.RemortHostType;
                                serviceStatus.RemortHostDescription = remortHostName.RemortHostDescription;
                                serviceStatus.Parent = remortHostName.Parent;
                                serviceStatus.SqlServerStatus = "Error";
                                serviceStatus.SqlServerStartupType = "";
                                serviceStatus.SqlServerAgnetStatus = "Error";
                                serviceStatus.SqlServerAgnetStartupType = "";
                                serviceStatus.IsRunning = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            serviceStatus.RemortHostName = remortHostName.RemortTPOSHostName;
                            serviceStatus.RemortHostType = remortHostName.RemortHostType;
                            serviceStatus.RemortHostDescription = remortHostName.RemortHostDescription;
                            serviceStatus.Parent = remortHostName.Parent;
                            serviceStatus.SqlServerStatus = "Error";
                            serviceStatus.SqlServerStartupType = "";
                            serviceStatus.SqlServerAgnetStatus = "Error";
                            serviceStatus.SqlServerAgnetStartupType = "";
                            serviceStatus.IsRunning = 0;
                            //TextLogger.LogToText(LoogerType.Error, $"Error connecting to : {remortHostName}. Exception : {ex.Message}");
                        }
                        finally
                        {
                            _httpClient.Dispose();

                        }
                    }
                }
                else
                {
                    //Client API is not working or machine is down
                    serviceStatus.RemortHostName = remortHostName.RemortTPOSHostName;
                    serviceStatus.RemortHostType = remortHostName.RemortHostType;
                    serviceStatus.RemortHostDescription = remortHostName.RemortHostDescription;
                    serviceStatus.Parent = remortHostName.Parent;
                    serviceStatus.SqlServerStatus = "Error";
                    serviceStatus.SqlServerStartupType = "";
                    serviceStatus.SqlServerAgnetStatus = "Error";
                    serviceStatus.SqlServerAgnetStartupType = "";
                    serviceStatus.IsRunning = 0;
                }
            }
            catch (Exception ex)
            {
                //TextLogger.LogToText(LoogerType.Error, $"Error reading status for the Remorte Devoce {ex.ToString()}");
                serviceStatus.RemortHostName = remortHostName.RemortTPOSHostName;
                serviceStatus.RemortHostType = remortHostName.RemortHostType;
                serviceStatus.RemortHostDescription = remortHostName.RemortHostDescription;
                serviceStatus.Parent = remortHostName.Parent;
                serviceStatus.SqlServerStatus = "Error";
                serviceStatus.SqlServerStartupType = "";
                serviceStatus.SqlServerAgnetStatus = "Error";
                serviceStatus.SqlServerAgnetStartupType = "";
                serviceStatus.IsRunning = 0;
            }
            Console.Write(".");
            return serviceStatus;

        }

        private static async Task<bool> PingWithHttpClient(string uri)
        {
            string hostUrl = uri;
            try
            {
                using (HttpClient httpClient = new())
                {
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        RequestUri = new Uri(hostUrl),
                        Method = HttpMethod.Head
                    };
                    httpClient.Timeout = TimeSpan.FromSeconds(GlobalStaticVariables.APIHealthCheckEndpointTimeout);
                    var result = await httpClient.SendAsync(request);
                    return result.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                return false;
            }
        }

    }
}

