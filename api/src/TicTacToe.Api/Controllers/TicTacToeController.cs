using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicTacToe.Core;
using TicTacToe.Core.Models;

namespace TicTacToe.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicTacToeController : ControllerBase
    {
        private readonly ILogger<TicTacToeController> _logger;
        private readonly ITicTacToeManager _manager;

        public TicTacToeController(ILogger<TicTacToeController> logger, ITicTacToeManager manager)
        {
            _logger = logger;
            _manager = manager;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<GameState>> GetGameStateAsync(Guid gameId)
        {
            //TODO: Validate input
            var result = await _manager.GetGameStateAsync(gameId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> SaveGameStateAsync(GameState gameState)
        {
            //TODO: Validate input

            var result = await _manager.SaveGameStateAsync(gameState);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
