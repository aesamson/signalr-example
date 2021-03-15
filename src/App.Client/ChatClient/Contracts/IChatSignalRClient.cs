using System;
using System.Threading.Tasks;

namespace App.Client.ChatClient.Contracts
{
    internal interface IChatSignalRClient
    {
        Task Connect();

        Task PostMessage(string message, string group);
        
        void Subscribe<T>(string method, Action<T> handler);

        Task Disconnect();
    }
}