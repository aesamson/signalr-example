using System;
using System.Threading.Tasks;
using App.Client.Services.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace App.Client.ChatClient
{
    internal abstract class ChatSignalRClientBase
    {
        private readonly ISignalRClientBuilder _connectionBuilder;
        protected readonly ILogger Logger;
        private HubConnection _connection;

        protected ChatSignalRClientBase(ISignalRClientBuilder connectionBuilder, ILogger logger)
        {
            _connectionBuilder = connectionBuilder;
            Logger = logger;
        }

        protected virtual void Execute(Action<HubConnection> handler)
        {
            if (_connection == null)
                _connection = _connectionBuilder.Build();

            try
            {
                handler.Invoke(_connection);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "System error");
                throw;
            }
        }
        
        protected virtual Task Execute(Func<HubConnection, Task> handler)
        {
            if (_connection == null)
                _connection = _connectionBuilder.Build();
            
            try
            {
                return handler.Invoke(_connection);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "System error");
                throw;
            }
        }
    }
}