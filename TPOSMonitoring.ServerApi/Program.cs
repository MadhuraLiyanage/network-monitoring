using Microsoft.EntityFrameworkCore;
using Serilog;
using TPOSMonitoring.ServerApi;

//var builder = WebApplication.CreateBuilder(args);

var webApplicationOptions = new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory,
    ApplicationName = System.Diagnostics.Process.GetCurrentProcess().ProcessName
};
var builder = WebApplication.CreateBuilder(webApplicationOptions);

//Make API a Windows Service
builder.Host.UseWindowsService();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//logger
Log.Logger = new LoggerConfiguration().WriteTo.File(builder.Configuration.GetValue<string>("GlobalVariables:LogFilePath"), rollingInterval: RollingInterval.Day).CreateLogger();

GlobalStaticVariables.DbConnectionString = builder.Configuration.GetConnectionString("DBConnection");
GlobalStaticVariables.Interval = int.Parse(builder.Configuration.GetValue<string>("GlobalVariables:Interval"));
GlobalStaticVariables.Token = builder.Configuration.GetValue<string>("GlobalVariables:Token");
GlobalStaticVariables.RemorteHostStatusUri = builder.Configuration.GetValue<string>("GlobalVariables:RemorteHostStatusUri"); 
GlobalStaticVariables.RemortHostStatusEndpint = builder.Configuration.GetValue<string>("GlobalVariables:RemortHostStatusEndpint");
GlobalStaticVariables.APIHealthCheckEndpoint = builder.Configuration.GetValue<string>("GlobalVariables:APIHealthCheckEndpoint");
GlobalStaticVariables.APIHealthStatusCheckEndpointTimeout = int.Parse(builder.Configuration.GetValue<string>("GlobalVariables:APIHealthStatusCheckEndpointTimeout"));
GlobalStaticVariables.APIHealthCheckEndpointTimeout = int.Parse(builder.Configuration.GetValue<string>("GlobalVariables:APIHealthCheckEndpointTimeout"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
