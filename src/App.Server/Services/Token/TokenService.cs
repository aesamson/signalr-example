using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Server.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App.Server.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly AuthOptions _options;

        public TokenService(IOptionsSnapshot<AuthOptions> options)
        {
            _options = options.Value;
        }

        public string IssueToken(string nick)
        {
            var securityKey = Encoding.UTF8.GetBytes(_options.SecurityKey);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, nick), 
                }),
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),  SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}