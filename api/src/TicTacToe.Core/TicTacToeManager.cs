using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicTacToe.Core.Models;
using TicTacToe.Core.Repositories;

namespace TicTacToe.Core
{
    public class TicTacToeManager : ITicTacToeManager
    {
        private readonly ILogger<TicTacToeManager> _logger;
        private readonly ITicTacToeRepository _repository;

        public TicTacToeManager(ILogger<TicTacToeManager> logger, ITicTacToeRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<GameState> GetGameStateAsync(Guid gameId)
        {
            return await _repository.GetGameStateAsync(gameId);
        }

        public async Task<bool> SaveGameStateAsync(GameState gameState)
        {
            return await _repository.SaveGameStateAsync(gameState);
        }
    }
}
