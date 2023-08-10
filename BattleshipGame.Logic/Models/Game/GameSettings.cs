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
            GenerateShips();
            DrawShipSettings();
        }

        public void GenerateFields()
        {
            for (int x = 0; x < 10; i++)
            {
                for (int y = 0; y < 10; y++)
                {
                    new Field(x, y);
                }
            }
        }

        public void GenerateShips()
        {
            for (int i = 0; i < 5; i++)
            {
                ShipList.Add(new Ship(1));
            }

            for (int i = 0; i < 4; i++)
            {
                ShipList.Add(new Ship(2));
            }

            for (int i = 0; i < 3; i++)
            {
                ShipList.Add(new Ship(3));
            }

            for (int i = 0; i < 2; i++)
            {
                ShipList.Add(new Ship(4));
            }

            for (int i = 0; i < 1; i++)
            {
                ShipList.Add(new Ship(5));
            }
        }

        public void DrawShipSettings()
        {
            Random random = new Random();
            foreach (var ship in ShipList)
            {
                DeployShip(ship, random);
            }
        }

        public void DeployShip(Ship ship, Random randomSpot)
        {
            while (true)
            {
                int startX = randomSpot.Next(0, XField);
                int startY = randomSpot.Next(0, YField);
                int endX = 0;
                int endY = 0;

                if (ship.Size == 1)
                {
                    UpdateField(ship, startX, startY, endX, endY);
                    CanPlaceShip(startX, startY, endX, endY);
                }

                //if (ship.Size == 2)
                //{
                //    Random random = new Random();
                //    int coordinate = random.Next(0, 1);

                //    if (coordinate == 0)
                //    {
                //        endX = startX + 1;
                //        endY = startY;
                //    }
                //    endY = startY + 1;
                //    endX = startX;

                //    if (startX != endX && startY != endY)
                //    {
                //        UpdateField(ship, startX, startY, endX, endY);
                //        CanPlaceShip(startX, startY, endX, endY);
                //        break;
                //    }

                //    // Starts this method again.
                //    DeployShip(ship, randomSpot);
                //}
            }
        }

        public bool UpdateField(Ship ship, int startX, int startY, int endX, int endY)
        {
            bool result = false;

            if (ship.Size == 1)
            {
                if (Fields.Contains(startX) == false
                    || YFields.Contains(startY) == false) result = false;

                XFields.RemoveAt(startX);
                YFields.RemoveAt(startY);
                result = true;
            }

            return result;

            //if (ship.Size == 2)
            //{
            //    if (XFields.GetRange()
            //}
        }

        public bool CanPlaceShip(int startX, int startY, int endX, int endY)
        {

        }
    }
}
