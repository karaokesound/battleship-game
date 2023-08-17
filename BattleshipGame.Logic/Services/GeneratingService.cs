using BattleshipGame.API.Models.Game;
using System.Text;

namespace BattleshipGame.Logic.Services
{
    public class GeneratingService : IGeneratingService
    {
        public List<Field> GenerateFields(int XFields, int YFields, string player)
        {
            List<Field> fieldsList = new List<Field>();

            for (int x = 0; x < XFields; x++)
            {
                for (int y = 0; y < YFields; y++)
                {
                    fieldsList.Add(new Field(x, y, player));
                }
            }

            return fieldsList;
        }

        public List<Ship> GenerateShips(int numberOfShips)
        {
            List<Ship> shipsList = new List<Ship>();

            // Returns empty list

            if (numberOfShips < 5) return shipsList;

            // Returns 1-field ships

            for (int i = 0; i < 5; i++)
            {
                shipsList.Add(new Ship(1));
            }

            if (shipsList.Count >= numberOfShips) return shipsList;

            // Returns 2-field ships

            for (int i = 0; i < 4; i++)
            {
                shipsList.Add(new Ship(2));
            }

            if (shipsList.Count >= numberOfShips) return shipsList;

            // Returns 3-field ships

            for (int i = 0; i < 3; i++)
            {
                shipsList.Add(new Ship(3));
            }

            if (shipsList.Count >= numberOfShips) return shipsList;

            // Returns 4-field ships

            for (int i = 0; i < 2; i++)
            {
                shipsList.Add(new Ship(4));
            }

            if (shipsList.Count >= numberOfShips) return shipsList;

            return shipsList;
        }

        public string GenerateGameBoard(List<Field> fields, int XFields, int YFields)
        {
            var gameBoard = new StringBuilder();

            for (int y = 0; y < YFields; y++)
            {
                for (int x = 0; x < XFields; x++)
                {
                    var field = fields.FirstOrDefault(f => f.X == x && f.Y == y);
                    if (field != null && !field.IsEmpty)
                    {
                        gameBoard.Append("1 ");
                    }
                    else
                    {
                        gameBoard.Append("0 ");
                    }
                }
                gameBoard.AppendLine();
            }

            return gameBoard.ToString();
        }
    }
}
