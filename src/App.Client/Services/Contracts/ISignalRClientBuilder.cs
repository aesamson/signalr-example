using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace App.Client.Services.Contracts
{
    internal interface ISignalRClientBuilder
    {
        HubConnection Build(HttpTransportType transport = HttpTransportType.WebSockets);
    }
}