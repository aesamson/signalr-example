using System.Threading.Tasks;
using App.Server.Database;
using App.Server.Models;
using App.Server.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace App.Server.Controllers
{
    [Route("/api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly ChatOptions _options;
        private readonly IDataStorage _storage;

        public StateController(IOptionsSnapshot<ChatOptions> options, IDataStorage storage)
        {
            _storage = storage;
            _options = options.Value;
        }

        [HttpGet("initial")]
        public IActionResult Initial()
        {
            var state = new InitialState
            {
                ChatId = _options.ChatId
            };

            return Ok(state);
        }
        
        [HttpGet("history")]
        public async Task<IActionResult> History()
        {
            var state = await _storage.GetAllMessages();

            return Ok(state);
        }
    }
}