namespace BattleshipGame.API.Models.Game
{
    public class GameSettings
    {
        public int XField { get; set; }

        public int YField { get; set; }

        public List<Field> Fields { get; set; }

        public int NumberOfShips { get; set; }

        public ICollection<Ship> ShipList { get; set; }

        public GameSettings(int xField, int yField, int numberOfShips)
        {
            XField = xField;
            YField = yField;
            NumberOfShips = numberOfShips;
            Fields = new List<Field>();
            ShipList = new List<Ship>();

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
                    int startY = random.Next(0, YField);
                    int endX = 0;
                    int endY = 0;

                    if (ship.Size == 1)
                    {
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
            var selectedField = Fields.FirstOrDefault(xy => xy.X == startX && xy.Y == startY);

            // Generates random coordinates again if selected field is occupied
            // or validation is false.
            if (selectedField.IsEmpty == false || selectedField.IsValid == false)
            {
                return false;
            }

            // Takes selected field and marked as occupied if validation is true.
            selectedField.IsEmpty = false;

            // Inalidates neighboring fields.
            var invalidFields = Fields.FindAll(xy => ((xy.X == selectedField.X + 1 || xy.X == selectedField.X - 1) && xy.Y == selectedField.Y)
            || ((xy.Y == selectedField.Y - 1 || xy.Y == selectedField.Y + 1) && xy.X == selectedField.X)
            || (xy.X == selectedField.X - 1 && xy.Y == selectedField.Y - 1)
            || (xy.X == selectedField.X - 1 && xy.Y == selectedField.Y + 1)
            || (xy.X == selectedField.X + 1 && xy.Y == selectedField.Y - 1)
            || (xy.X == selectedField.X + 1 && xy.Y == selectedField.Y + 1)
            && xy.IsValid == true
            && xy.IsEmpty == true)
                .ToList();

            foreach (var field in invalidFields)
            {
                field.IsValid = false;
            }

            return true;
        }

        public void DisplayShipsCoordinatesAndGameBoard()
        {
            int shipCounter = 0;

            foreach (var field in Fields)
            {
                if (field.IsEmpty == true)
                {
                    continue;
                }

                Console.WriteLine($"Ship {shipCounter} coordinate (X, Y): {field.X}, {field.Y}");
                shipCounter++;
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
