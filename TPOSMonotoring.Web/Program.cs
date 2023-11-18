using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.WindowsServices;
using Serilog;
using System.Configuration;
using TPOSMonotoring.Web.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
        {
            ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
            ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
            Args = args
        });
        builder.Host.UseWindowsService();

        //var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        GlobalSettings globalSettings;

        // Get the current configuration file.
        var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        var section = config.GetSection(nameof(GlobalSettings));
        var globalConfig = section.Get<GlobalSettings>();

        GlobalStaticSettings.Interval = globalConfig.Interval;
        GlobalStaticSettings.LogFilePath = globalConfig.LogFilePath;
        GlobalStaticSettings.Token = globalConfig.Token;
        GlobalStaticSettings.MonitoringURI = globalConfig.MonitoringURI;
        GlobalStaticSettings.MonitoringEndpoint = globalConfig.MonitoringEndpoint;


        //logger
        Log.Logger = new LoggerConfiguration().WriteTo.File(builder.Configuration.GetValue<string>("GlobalSettings:LogFilePath"), rollingInterval: RollingInterval.Day).CreateLogger();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }


        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}