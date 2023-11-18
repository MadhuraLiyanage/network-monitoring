using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.JSInterop;
using TPOSMonitoring.Models;
using TPOSMonotoring.Web.Data;
using Monitor = TPOSMonotoring.Web.Data.Monitor;

namespace TPOSMonotoring.Web.Pages
{
    public class NetworkHomeBase: ComponentBase
    {
        public IEnumerable<ServiceStatus> ServiceStatuses { get; set; } = null;
        private static readonly Monitor _monitor = new Monitor();

        /*protected override async void OnInitialized()
        {
            base.OnInitialized();
            await ServerStatusListAsync();
            StateHasChanged();
        //    return;
        }*/

        protected override async Task OnInitializedAsync()
        {

            base.OnInitialized();

            var timer = new System.Threading.Timer((_) =>
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
            //HQServer - Layer 0, RegServer - Layer 1, TPOS - Layer 2
        }
    }
}
