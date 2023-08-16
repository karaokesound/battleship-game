using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public class GeneratingService : IGeneratingService
    {
        public List<Field> GenerateFields(int XFields, int YFields)
        {
            List<Field> fieldsList = new List<Field>();

            for (int x = 0; x < XFields; x++)
            {
                for (int y = 0; y < YFields; y++)
                {
                    fieldsList.Add(new Field(x, y));
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

        public void GenerateGameBoard(List<Field> fields, int XFields, int YFields)
        {
            fields = fields.OrderBy(c => c.ShipSize).ToList();

            int shipsCounter = 0;

            // Displays ships coordinates

            foreach (var field in fields)
            {
                if (field.IsEmpty)
                {
                    continue;
                }

                if (!field.IsEmpty && field.ShipSize == 1)
                {
                    Console.WriteLine($"1-field ship {shipsCounter} coordinate (X, Y): {field.X}, {field.Y}");
                    shipsCounter++;
                }
                else if (!field.IsEmpty && field.ShipSize == 2)
                {
                    Console.WriteLine($"2-field ship {shipsCounter} coordinates (X, Y): {field.X}, {field.Y}");
                    shipsCounter++;
                }
                else if (!field.IsEmpty && field.ShipSize == 3)
                {
                    Console.WriteLine($"3-field ship {shipsCounter} coordinates (X, Y): {field.X}, {field.Y}");
                    shipsCounter++;
                }
            }

            Console.WriteLine();

            // Generates game board

            for (int y = 0; y < YFields; y++)
            {
                for (int x = 0; x < XFields; x++)
                {
                    var field = fields.FirstOrDefault(f => f.X == x && f.Y == y);

                    if (field != null && !field.IsEmpty)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.Write("1 ");
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.Write("0 ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
