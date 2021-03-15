using System;
using System.Threading.Tasks;
using App.Server.Options;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace App.Server.Hubs
{
    public abstract class ChatHubBase : Hub
    {
        private readonly ChatOptions _options;
        
        protected string ClientNick => Context.User.Identity.Name;
        protected ChatHubBase(IOptions<ChatOptions> options)
        {
            _options = options.Value;
        }
        
        protected string ConnectionId => Context.ConnectionId;

        public override async Task OnConnectedAsync()
        {
            // построение групп для чата
            await Groups.AddToGroupAsync(ConnectionId, _options.ChatId);
            
            await Clients.OthersInGroup(_options.ChatId).SendAsync("joined", new
            {
                Group = _options.ChatId, Nick = ClientNick
            });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // выход из конкретного чата
            await Groups.RemoveFromGroupAsync(ConnectionId, _options.ChatId);
            
            await Clients.OthersInGroup(_options.ChatId).SendAsync("lost", new
            {
                Group = _options.ChatId, Nick = ClientNick
            });
        }
    }
}