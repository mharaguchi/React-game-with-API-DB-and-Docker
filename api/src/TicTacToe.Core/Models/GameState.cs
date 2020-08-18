using System;

namespace TicTacToe.Core.Models
{
    public class GameState
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string History { get; set; }
        public int TurnNumber { get; set; }
    }
}
