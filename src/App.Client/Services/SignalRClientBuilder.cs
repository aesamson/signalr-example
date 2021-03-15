using App.Client.Services.Contracts;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

namespace App.Client.Services
{
    internal sealed class SignalRClientBuilder : ISignalRClientBuilder
    {
        private readonly ITokenAccessor _tokenAccessor;
        private readonly IConfiguration _configuration;
        
        private readonly AppSettings _settings;

        public SignalRClientBuilder(ITokenAccessor tokenAccessor, IConfiguration configuration, 
            IOptions<AppSettings> options)
        {
            _tokenAccessor = tokenAccessor;
            _configuration = configuration;
            _settings = options.Value;
        }

        public HubConnection Build(HttpTransportType transport = HttpTransportType.WebSockets)
        {
            return new HubConnectionBuilder()
                .WithUrl($"{_settings.ServerUrl}/signalr/chat", opt =>
                {
                    opt.AccessTokenProvider = async () => await _tokenAccessor.Fetch();
                    opt.Transports = transport;
                })
                .ConfigureLogging(opt =>
                {
                    opt.AddNLog();
                    opt.AddConfiguration(_configuration.GetSection("Logging"));
                })
                .WithAutomaticReconnect()
                .Build();
        }
    }
}