using BattleshipGame.API.Models.Game;
using BattleshipGame.Logic.Models.Game;
using BattleshipGame.Logic.Services;
using System.Text;

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

        private readonly IGeneratingService _generatingService;

        public GameCore
            (int xField, int yField, int numberOfShips,
            IValidationService validation,
            IGeneratingService generatingService)
        {
            XField = xField;
            YField = yField;
            NumberOfShips = numberOfShips;
            Fields = new List<Field>();
            ShipList = new List<Ship>();
            _validation = validation;
            _generatingService = generatingService;

            GenerateFields();
            GenerateShips(NumberOfShips);
            GenerateRandomCoordinates();
            DisplayShipsCoordinatesAndGameBoard();
        }

        public void GenerateFields()
        {
            Fields = _generatingService.GenerateFields(XField, YField);
        }

        public void GenerateShips(int numberOfShips)
        {
            ShipList = _generatingService.GenerateShips(numberOfShips);
        }

        public void GenerateRandomCoordinates()
        {
            Random random = new Random();

            foreach (var ship in ShipList)
            {
                int tryCount = 0;

                while (tryCount < 1000)
                {
                    int startX = random.Next(0, XField);
                    int startY = random.Next(0, XField);
                    int endX = 0;
                    int endY = 0;
                    bool mixedDirection = random.Next(2) == 0 ? true : false;
                    int XDirection = random.Next(2) == 0 ? 1 : -1;
                    int YDirection = random.Next(2) == 0 ? 1 : -1;
                    bool verticalHorizontal = random.Next(4) != 0; // 2/3 szansy na pionowy kierunek
                    int tripleShipDirection = random.Next(2) == 0 ? 1 : -1;

                    if (ship.Size == 4)
                    {
                        // horizontal

                        if (!verticalHorizontal && !mixedDirection)
                        {
                            if (startX > 3)
                            {
                                endX = startX - 3;
                                endY = startY;
                            }

                            endX = startX + 3;
                            endY = startY;
                        }

                        // vertical

                        else if (verticalHorizontal && !mixedDirection)
                        {
                            if (startY > 3)
                            {
                                endY = startY - 3;
                                endX = startX;
                            }

                            endY = startY + 3;
                            endX = startX;
                        }

                        if (ValidateFields(ship, startX, startY, endX, endY))
                        {
                            break;
                        }
                        tryCount++;
                    }
                    if (ship.Size == 3)
                    {
                        if (!verticalHorizontal && tripleShipDirection == 1)
                        {
                            endX = startX + XDirection * 2;
                            endY = startY;
                        }
                        else if (!verticalHorizontal && tripleShipDirection == -1)
                        {
                            endX = startX + XDirection;
                            endY = startY + YDirection;
                        }
                        else if (verticalHorizontal && tripleShipDirection == 1)
                        {
                            endX = startX;
                            endY = startY + YDirection * 2;
                        }
                        else if (verticalHorizontal && tripleShipDirection == -1)
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
                    else if (ship.Size == 1)
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
            if (ship.Size == 1)
            {
                return _validation.OneFieldShipValidation(startX, startY, endX, endY, Fields);
            }

            if (ship.Size == 2)
            {
                return _validation.TwoFieldShipValidation(startX, startY, endX, endY, Fields);
            }

            if (ship.Size == 3)
            {
                return _validation.ThreeFieldShipValidation(startX, startY, endX, endY, Fields);
            }

            if (ship.Size == 4)
            {
                return _validation.FourFieldShipValidation(startX, startY, endX, endY, Fields);
            }

            return false;
        }

        public string DisplayShipsCoordinatesAndGameBoard()
        {
            int countSingleShips = 0;
            int countDoubleShips = 0;
            int countTripleShips = 0;
            int countQuadrupleShips = 0;

            foreach (var field in Fields)
            {
                if (field.ShipSize == 1) countSingleShips += 1;
                else if (field.ShipSize == 2) countDoubleShips += 1;
                else if (field.ShipSize == 3) countTripleShips += 1;
                else if (field.ShipSize == 4) countQuadrupleShips += 1;
            }

            if (countSingleShips != 5 || countDoubleShips != 8 || countTripleShips != 9)
            {
                GenerateRandomCoordinates();
                DisplayShipsCoordinatesAndGameBoard();
            }

            return _generatingService.GenerateGameBoard(Fields, XField, YField);
        }
    }
}
