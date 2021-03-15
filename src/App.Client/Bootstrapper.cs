using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.Client
{
    internal sealed class Bootstrapper : IHostedService
    {
        private readonly Application _application;
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly AppSettings _settings;
        private readonly ILogger _logger;

        public Bootstrapper(Application application, IOptions<AppSettings> settings, 
            IHostApplicationLifetime applicationLifetime, ILogger<Bootstrapper> logger)
        {
            _application = application;
            _applicationLifetime = applicationLifetime;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _application.Run();
            }
            catch (Exception ex)
            {
                _logger.LogError($"The application is terminated with error: {ex.Message}");
            }
            
            _applicationLifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}