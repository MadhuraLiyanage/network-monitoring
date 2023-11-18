using Microsoft.AspNetCore.Components;
using TPOSMonitoring.Models;
using TPOSMonotoring.Web.Data;
using TPOSMonitoring.Enum;
using Monitor = TPOSMonotoring.Web.Data.Monitor;

namespace TPOSMonotoring.Web.Pages
{
    public class DashboardBase: ComponentBase
    {
        public IEnumerable<ServiceStatus> ServiceStatuses { get; set; } = null;
        private static readonly Monitor _monitor = new Monitor();

        /*protected override async void OnInitialized()
        {
            base.OnInitialized();
            ServerStatusListAsync();
            //return;
        }*/

        protected override async Task OnInitializedAsync()
        {

            var timer = new Timer((_) =>
            {
                InvokeAsync(async () =>
                {
                   ServerStatusListAsync();
                });
            }, null, 0, GlobalStaticSettings.Interval);
  

        }



        private async void ServerStatusListAsync()
        {
            ServiceStatuses = await _monitor.GetNetworkStatus();
            StateHasChanged();
            StateHasChanged();
            //TextLogger.LogToText(LoogerType.Information, $"ServerStatusListAsync method : {DateTime.Now}");

            //HQServer - Layer 0, RegServer - Layer 1, TPOS - Layer 2
        }
    }
}
