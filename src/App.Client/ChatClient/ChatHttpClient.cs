using System.Net.Http;
using System.Threading.Tasks;
using App.Client.ChatClient.Contracts;
using App.Client.ChatClient.Models;
using Newtonsoft.Json;

namespace App.Client.ChatClient
{
    internal sealed class ChatHttpClient : IChatHttpClient
    {
        private readonly HttpClient _httpClient;

        public ChatHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<InitialState> GetInitialState()
        {
            var response = await _httpClient.GetAsync($"/api/state/initial");
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<InitialState>(content);
        }

        public async Task<Message[]> GetHistory()
        {
            var response = await _httpClient.GetAsync($"/api/state/history");
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Message[]>(content);
        }
    }
}