using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using App.Client.Services.Contracts;
using Microsoft.Extensions.Options;

namespace App.Client.Services
{
    internal sealed class TokenAccessor : ITokenAccessor
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppSettings _settings;
            
        private static readonly SemaphoreSlim Semaphore;
        private static string _token;

        static TokenAccessor()
        {
            Semaphore = new SemaphoreSlim(1, 1);
        }

        public TokenAccessor(IHttpClientFactory clientFactory, IOptions<AppSettings> options)
        {
            _clientFactory = clientFactory;
            _settings = options.Value;
        }

        public async Task<string> Fetch()
        {
            if (!string.IsNullOrEmpty(_token))
                return _token;

            try
            {
                await Semaphore.WaitAsync();
                
                if (!string.IsNullOrEmpty(_token))
                    return _token;

                using (var client = _clientFactory.CreateClient())
                {
                    client.BaseAddress = new Uri(_settings.ServerUrl);
                    var response = await client.GetAsync($"/api/auth/token?nick={ApplicationStorage.Nickname}");
                    _token = await response.Content.ReadAsStringAsync();
                }

                return _token;
            }
            finally
            {
                Semaphore.Release();
            }
        }
    }
}