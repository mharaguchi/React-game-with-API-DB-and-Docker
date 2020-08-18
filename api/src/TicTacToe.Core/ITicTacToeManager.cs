using System;
using System.Threading.Tasks;
using TicTacToe.Core.Models;

namespace TicTacToe.Core
{
    public interface ITicTacToeManager
    {
        Task<GameState> GetGameStateAsync(Guid gameId);

        Task<bool> SaveGameStateAsync(GameState gameState);
    }
}
