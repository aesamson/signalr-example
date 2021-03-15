using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using App.Client.Services.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace App.Client.DelegatingHandlers
{
    internal sealed class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenAccessor _tokenAccessor;

        public AuthDelegatingHandler(ITokenAccessor tokenAccessor)
        {
            _tokenAccessor = tokenAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenAccessor.Fetch();

            request.Headers.Authorization =
                new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}