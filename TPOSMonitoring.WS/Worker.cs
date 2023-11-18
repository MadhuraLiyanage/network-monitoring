using TPOSMonitoring.Models;
using TPOSMonitoring.WS.Enum;
using TPOSMonitoring.WS.Services;
using System.Net.Http;

namespace TPOSMonitoring.WS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly CheckStatus _processSkyBuysFile = new CheckStatus();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");

                try
                {
                    _processSkyBuysFile.SendSkyBuysFile();
                }
                catch (Exception ex)
                {
                    TextLogger.LogToText(LoogerType.Error, $"Error reading Worker Service execution times. Exception : {ex.Message}");
                }

                await Task.Delay(GlobalStaticVaiables.Interval, stoppingToken);
            }
        }
    }
}