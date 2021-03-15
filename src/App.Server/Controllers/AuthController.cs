using App.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Server.Controllers
{
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("token")]
        [AllowAnonymous]
        public IActionResult Issue([FromQuery] string nick)
        {
            var token = _tokenService.IssueToken(nick);

            return Ok(token);
        }
    }
}