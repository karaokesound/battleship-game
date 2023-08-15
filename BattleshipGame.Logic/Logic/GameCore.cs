using BattleshipGame.API.Models.Game;
using BattleshipGame.Logic.Services;

namespace BattleshipGame.Logic.Logic
{
    public class GameCore
    {
        public int XField { get; set; }

        public int YField { get; set; }

        public List<Field> Fields { get; set; }

        public int NumberOfShips { get; set; }

        public ICollection<Ship> ShipList { get; set; }

        private readonly IValidationService _validation;

        public GameCore(int xField, int yField, int numberOfShips,
            IValidationService validation)
        {
            XField = xField;
            YField = yField;
            NumberOfShips = numberOfShips;
            Fields = new List<Field>();
            ShipList = new List<Ship>();

            _validation = validation;
            GenerateFields();
            GenerateRandomCoordinates();
            DisplayShipsCoordinatesAndGameBoard();
        }

        public void GenerateFields()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Fields.Add(new Field(x, y));
                }
            }

            GenerateShips(NumberOfShips);
        }

        public void GenerateShips(int numberOfShips)
        {
            if (numberOfShips < 5) return;

            for (int i = 0; i < 5; i++)
            {
                ShipList.Add(new Ship(1));
            }

            if (ShipList.Count >= numberOfShips) return;

            for (int i = 0; i < 4; i++)
            {
                ShipList.Add(new Ship(2));
            }

            if (ShipList.Count >= numberOfShips) return;

            for (int i = 0; i < 3; i++)
            {
                ShipList.Add(new Ship(3));
            }

            if (ShipList.Count >= numberOfShips) return;

            for (int i = 0; i < 2; i++)
            {
                ShipList.Add(new Ship(4));
            }

            if (ShipList.Count >= numberOfShips) return;

            for (int i = 0; i < 1; i++)
            {
                ShipList.Add(new Ship(5));
            }

            if (ShipList.Count >= numberOfShips) return;
        }

        public void GenerateRandomCoordinates()
        {
            Random random = new Random();
            foreach (var ship in ShipList)
            {
                int tryCount = 0;

                // Generates random coordinates and tries to validate it.
                while (tryCount < 30)
                {
                    int startX = random.Next(0, XField);
                    int startY = random.Next(0, XField);
                    int endX = 0;
                    int endY = 0;
                    int XDirection = random.Next(2) == 0 ? 1 : -1;
                    int YDirection = random.Next(2) == 0 ? 1 : -1;
                    bool verticalHorizontal = random.Next(2) == 0 ? true : false;

                    if (ship.Size == 1)
                    {
                        if (ValidateFields(ship, startX, startY, endX, endY))
                        {
                            break;
                        }
                        tryCount++;
                        continue;
                    }

                    if (ship.Size == 2)
                    {

                        // horizontal
                        if (XDirection == -1 && startX > 0 && verticalHorizontal)
                        {
                            endX = startX - 1;
                            endY = startY;

                            if (ValidateFields(ship, startX, startY, endX, endY))
                            {
                                break;
                            }
                            tryCount++;
                            continue;
                        }

                        endX = startX + 1;
                        endY = startY;

                        if (ValidateFields(ship, startX, startY, endX, endY))
                        {
                            break;
                        }
                        tryCount++;

                        // vertical
                        if (YDirection == -1 && startX > 0 && !verticalHorizontal)
                        {
                            endX = startX;
                            endY = startY - 1;

                            if (ValidateFields(ship, startX, startY, endX, endY))
                            {
                                break;
                            }
                            tryCount++;
                            continue;
                        }

                        endX = startX;
                        endY = startY + 1;

                        if (ValidateFields(ship, startX, startY, endX, endY))
                        {
                            break;
                        }
                        tryCount++;
                    }
                }
            }
        }

        public bool ValidateFields(Ship ship, int startX, int startY, int endX, int endY)
        {
            if (ship.Size == 1)
            {
                var selectedField = Fields.FirstOrDefault(xy => xy.X == startX && xy.Y == startY);

                return _validation.OneFieldShipValidation(selectedField, Fields);

            }

            if (ship.Size == 2)
            {
                var selectedFields = Fields.FindAll(f => f.X == startX && f.Y == startY || f.X == endX && f.Y == endY)
                    .ToList();

                return _validation.TwoFieldShipValidation(selectedFields, Fields);
            }

            return false;
        }

        public void DisplayShipsCoordinatesAndGameBoard()
        {
            int shipCounter = 0;

            foreach (var field in Fields)
            {
                if (field.IsEmpty)
                {
                    continue;
                }

                if (!field.IsEmpty && field.ShipSize == 1)
                {
                    Console.WriteLine($"1-field ship {shipCounter} coordinate (X, Y): {field.X}, {field.Y}");
                    shipCounter++;
                }
            }

            foreach (var field in Fields)
            {
                if (field.IsEmpty)
                {
                    continue;
                }

                if (!field.IsEmpty && field.ShipSize == 2)
                {
                    Console.WriteLine($"2-field ship {shipCounter} coordinates (X, Y): {field.X}, {field.Y}");
                    shipCounter++;
                }
            }

            Console.WriteLine();

            for (int y = 0; y < YField; y++)
            {
                for (int x = 0; x < XField; x++)
                {
                    var field = Fields.FirstOrDefault(f => f.X == x && f.Y == y);

                    if (field != null && !field.IsEmpty)
                    {
                        Console.Write("1 ");
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
