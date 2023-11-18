using TPOSMonitoring.PosApi;

//var builder = WebApplication.CreateBuilder(args);

//To run as a windows service (Desable when add-migration called)
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

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

GlobalStaticVariables.SqlServerServiceName = builder.Configuration.GetValue<string>("AppSettings:SqlServerServiceName");
GlobalStaticVariables.SqlServerAgentServiceName = builder.Configuration.GetValue<string>("AppSettings:SqlServerAgentServiceName");
GlobalStaticVariables.Token = builder.Configuration.GetValue<string>("AppSettings:Token");

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
