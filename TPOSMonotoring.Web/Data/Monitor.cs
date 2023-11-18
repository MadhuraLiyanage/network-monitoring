using Newtonsoft.Json;
using TPOSMonitoring.Enum;
using TPOSMonitoring.Models;

namespace TPOSMonotoring.Web.Data
{
    public class Monitor
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri(GlobalStaticSettings.MonitoringURI)
        };

        public async Task<IEnumerable<ServiceStatus>> GetNetworkStatus()
        {
            TextLogger.LogToText(LoogerType.Information, "Network monitoring");
            TextLogger.LogToText(LoogerType.Information, $"Call RESTful API enpoint [GET] {GlobalStaticSettings.MonitoringURI}{GlobalStaticSettings.MonitoringEndpoint}");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("token", GlobalStaticSettings.Token);

            try
            {
                var result = await _httpClient.GetAsync(GlobalStaticSettings.MonitoringEndpoint);

                if (result.IsSuccessStatusCode)
                {
                    var json = await result.Content.ReadAsStringAsync();

                    IEnumerable<ServiceStatus> serviceStatuses = JsonConvert.DeserializeObject<IEnumerable<ServiceStatus>>(json);
                    TextLogger.LogToText(LoogerType.Information, $"Received response : {result.StatusCode}");

                    return serviceStatuses;
                }
                else
                {
                    TextLogger.LogToText(LoogerType.Warning, $"Received response : {result.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                TextLogger.LogToText(LoogerType.Error, $"Error calling service monitoring server API. Exception : {ex.Message}");
            }

            return null;
        }
    }
}
