using System.Threading.Tasks;
using App.Server.Database;
using App.Server.Hubs.Messages;
using App.Server.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace App.Server.Hubs
{
    /// <summary>
    /// SignalR entry point
    /// </summary>
    [Authorize]
    public class ChatHub : ChatHubBase
    {
        private readonly IDataStorage _storage;

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="chatOptions"></param>
        public ChatHub(IDataStorage storage, IOptionsSnapshot<ChatOptions> chatOptions) : base(chatOptions)
        {
            _storage = storage;
        }
        
        /// <summary>
        /// Post message to chat
        /// </summary>
        /// <returns></returns>
        [HubMethodName("post")]
        public async Task PostMessage(PostMessage message)
        {
            await _storage.StoreMessage(message.Message, ClientNick);
            await Clients.OthersInGroup(message.Group).SendAsync("message", new
            {
                Message = message.Message,
                Group = message.Group,
                Nick = ClientNick
            });
        }
    }
}