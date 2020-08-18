using System;
using System.Threading.Tasks;
using TicTacToe.Core.Models;

namespace TicTacToe.Core.Repositories
{
    public interface ITicTacToeRepository
    {
        Task<GameState> GetGameStateAsync(Guid gameId);

        Task<bool> SaveGameStateAsync(GameState gameState);
    }
}
