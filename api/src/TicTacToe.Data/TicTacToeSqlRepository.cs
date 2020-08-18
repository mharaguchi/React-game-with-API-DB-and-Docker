using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TicTacToe.Core.Options;
using TicTacToe.Core.Repositories;

namespace TicTacToe.Data
{
    public class TicTacToeSqlRepository : ITicTacToeRepository
    {
        private readonly ILogger<TicTacToeSqlRepository> _logger;
        private readonly IOptions<TicTacToeOptions> _options;

        public TicTacToeSqlRepository(ILogger<TicTacToeSqlRepository> logger, IOptions<TicTacToeOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<Core.Models.GameState> GetGameStateAsync(Guid gameId)
        {
            _logger.LogDebug("Entered {class} {method}", nameof(TicTacToeSqlRepository), nameof(GetGameStateAsync));
            try
            {
                using (var connection = new SqlConnection(_options.Value.ConnectionString))
                {
                    var result = await connection.QuerySingleAsync<Core.Models.GameState>(_options.Value.GetGameStateStoredProcedureName, new { GameID = gameId.ToString() }, commandType: CommandType.StoredProcedure);
                    _logger.LogDebug("Return from {class} {method}, result: {result}", nameof(TicTacToeSqlRepository), nameof(GetGameStateAsync), result);
                    return result;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{class} {method}: Error getting game state {gameId}", nameof(TicTacToeSqlRepository), nameof(GetGameStateAsync), gameId);
                return null;
            }
        }

        public async Task<bool> SaveGameStateAsync(Core.Models.GameState gameState)
        {
            _logger.LogDebug("Entered {class} {method}", nameof(TicTacToeSqlRepository), nameof(SaveGameStateAsync));
            try
            {
                using (var connection = new SqlConnection(_options.Value.ConnectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(_options.Value.SaveGameStateStoredProcedureName, new { GameID = gameState.GameId, gameState.History, gameState.TurnNumber }, commandType: CommandType.StoredProcedure);
                    if (affectedRows == 1)
                    {
                        _logger.LogDebug("Returning from {class} {method} {affectedRows}", nameof(TicTacToeSqlRepository), nameof(SaveGameStateAsync), affectedRows);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error saving game state {gameState}", gameState);
                return false;
            }
        }
    }
}
