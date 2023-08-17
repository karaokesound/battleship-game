using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Models.Game
{
    public class Game
    {
        public string Player1 { get; set; } = null!;

        public string Player2 { get; set; } = null!;

        public List<Field>? Player1Board { get; set; }

        public List<Field>? Player2Board { get; set; }

        public Game()
        {
            Player1Board = new List<Field>();
            Player2Board = new List<Field>();
        }
    }
}
