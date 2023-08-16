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

                while (tryCount < 50)
                {
                    int startX = random.Next(0, XField);
                    int startY = random.Next(0, XField);
                    int endX = 0;
                    int endY = 0;
                    int XDirection = random.Next(2) == 0 ? 1 : -1;
                    int YDirection = random.Next(2) == 0 ? 1 : -1;
                    bool verticalHorizontal = random.Next(4) != 0; // 2/3 szansy na pionowy kierunek
                    int TripleShipDirection = random.Next(2) == 0 ? 1 : -1;

                    if (ship.Size == 1)
                    {
                        if (ValidateFields(ship, startX, startY, endX, endY))
                        {
                            break;
                        }
                        tryCount++;
                    }
                    else if (ship.Size == 2)
                    {
                        if (verticalHorizontal)
                        {
                            endX = startX + XDirection;
                            endY = startY;
                        }
                        else
                        {
                            endX = startX;
                            endY = startY + YDirection;
                        }

                        if (ValidateFields(ship, startX, startY, endX, endY))
                        {
                            break;
                        }
                        tryCount++;
                    }
                    else if (ship.Size == 3)
                    {
                        if (!verticalHorizontal && TripleShipDirection == 1)
                        {
                            endX = startX + XDirection * 2;
                            endY = startY;
                        }
                        else if (!verticalHorizontal && TripleShipDirection == -1)
                        {
                            endX = startX + XDirection;
                            endY = startY + YDirection;
                        }
                        else if (verticalHorizontal && TripleShipDirection == 1)
                        {
                            endX = startX;
                            endY = startY + YDirection * 2;
                        }
                        else if (verticalHorizontal && TripleShipDirection == -1)
                        {
                            endX = startX + XDirection;
                            endY = startY + YDirection;
                        }

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
                var selectedFields = Fields.FindAll(f => f.X == startX && f.Y == startY
                || f.X == endX && f.Y == endY)
                    .ToList();

                if (selectedFields.Count != 2) return false;

                return _validation.TwoFieldShipValidation(selectedFields, Fields);
            }

            if (ship.Size == 3)
            {
                var selectedFields = Fields.FindAll(f => (f.X == startX && f.Y == startY)
                || f.X == endX && f.Y == endY
                || f.X == ((startX + endX) / 2) && f.Y == endY
                || f.Y == ((startY + endY) / 2) && f.X == endX)
                    .ToList();

                if (selectedFields.Count != 3) return false;

                return _validation.ThreeFieldShipValidation(selectedFields, Fields);
            }

            return false;
        }

        public void DisplayShipsCoordinatesAndGameBoard()
        {
            int countSingle = 0;
            int countDouble = 0;
            int countTriple = 0;

            foreach (var vfield in Fields)
            {
                if (vfield.ShipSize == 1) countSingle += 1;
                else if (vfield.ShipSize == 2) countDouble += 1;
                else if (vfield.ShipSize == 3) countTriple += 1;
            }

            if (countSingle != 5 || countDouble != 8 || countTriple != 9)
            {
                GenerateRandomCoordinates();
                DisplayShipsCoordinatesAndGameBoard();
            }

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

            foreach (var field in Fields)
            {
                if (field.IsEmpty)
                {
                    continue;
                }

                if (!field.IsEmpty && field.ShipSize == 3)
                {
                    Console.WriteLine($"3-field ship {shipCounter} coordinates (X, Y): {field.X}, {field.Y}");
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
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.Write("1 ");
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    else if (field.IsEmpty && field.IsValid)
                    {
                        Console.Write("0 ");
                    }
                    else
                    {
                        Console.Write("$ ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
