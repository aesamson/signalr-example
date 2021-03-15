using System;
using System.Threading.Tasks;
using App.Client.ChatClient.Contracts;
using App.Client.Services.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace App.Client.ChatClient
{
    internal sealed class ChatSignalRClient : ChatSignalRClientBase, IChatSignalRClient
    {
        public ChatSignalRClient(ISignalRClientBuilder builder, ILogger<ChatSignalRClient> logger)
            : base(builder, logger)
        {
        }

        public async Task Connect()
        {
            await Execute(cnn => cnn.StartAsync());
        }

        public async Task PostMessage(string message, string group)
        {
            await Execute(cnn => cnn.InvokeAsync("post", new
            {
                Group = @group,
                Message = message
            }));
        }

        public async Task Disconnect()
        {
            await Execute(cnn => cnn.StopAsync());
        }

        public void Subscribe<T>(string method, Action<T> handler)
        {
            Execute(cnn =>
            {
                cnn.On<T>(method, handler);
            });
        }
    }
}