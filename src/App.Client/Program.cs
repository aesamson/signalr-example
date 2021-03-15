using System;
using System.Threading.Tasks;
using App.Client.ChatClient;
using App.Client.ChatClient.Contracts;
using App.Client.DelegatingHandlers;
using App.Client.Services;
using App.Client.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace App.Client
{
    internal sealed class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(config =>
                {
                    config.AddJsonFile("hostsettings.json", true, false);
                    config.AddEnvironmentVariables("SIGNALR_CLIENT_");
                    config.AddCommandLine(args);
                })
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.json", false, false);
                    config.AddEnvironmentVariables("SIGNALR_CLIENT_");
                    config.AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddLogging();
                    
                    services.AddHostedService<Bootstrapper>();
                    
                    services.AddSingleton<Application>();
                    services.AddSingleton<ITokenAccessor, TokenAccessor>();
                    services.AddSingleton<ISignalRClientBuilder, SignalRClientBuilder>();
                    services.AddSingleton<IChatSignalRClient, ChatSignalRClient>();
                    
                    services.AddSingleton<AuthDelegatingHandler>();
                    services.AddHttpClient<IChatHttpClient, ChatHttpClient>(opt =>
                        {
                            opt.BaseAddress = new Uri(context.Configuration.GetValue<string>("AppSettings:ServerUrl"));
                        })
                        .AddHttpMessageHandler<AuthDelegatingHandler>();
                    
                    services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));
                })
                .ConfigureLogging((context, config) =>
                {
                    config.AddNLog();
                    config.AddConfiguration(context.Configuration.GetSection("Logging"));
                })
                .UseConsoleLifetime()
                .Build();
 
            await host.RunAsync();
        }
    }
}