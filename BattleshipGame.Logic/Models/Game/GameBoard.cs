namespace BattleshipGame.Logic.Models.Game
{
    public class GameBoard
    {
        public List<List<int>> Board { get; set; }

        public GameBoard()
        {
            Board = new List<List<int>>();
        }
    }
}
