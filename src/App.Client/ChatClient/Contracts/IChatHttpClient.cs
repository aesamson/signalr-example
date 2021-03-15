using System.Threading.Tasks;
using App.Client.ChatClient.Models;

namespace App.Client.ChatClient.Contracts
{
    internal interface IChatHttpClient
    {
        Task<InitialState> GetInitialState();
        
        Task<Message[]> GetHistory();
    }
}