namespace TicTacToe.Core.Options
{
    public class TicTacToeOptions
    {
        public string ConnectionString { get; set; }
        public string SaveGameStateStoredProcedureName { get; set; }
        public string GetGameStateStoredProcedureName { get; set; }
    }
}
