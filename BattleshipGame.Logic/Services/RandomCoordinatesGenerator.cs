//using BattleshipGame.API.Models.Game;

//namespace BattleshipGame.Logic.Services
//{
//    public class RandomCoordinatesGenerator : IRandomCoordinatesGenerator
//    {
//        public List<Field> GenerateRandomCoordinates(ICollection<Ship> shipList, int XField, int YField)
//        {
//            List<Field> shipsCoordinates = new List<Field>();

//            Random random = new Random();
//            foreach (var ship in shipList)
//            {
//                shipsCoordinates.Clear();

//                // Generates random coordinates and tries to validate it.
//                int startX = random.Next(0, XField);
//                int startY = random.Next(0, XField);
//                int endX = 0;
//                int endY = 0;
//                int XDirection = random.Next(2) == 0 ? 1 : -1;
//                int YDirection = random.Next(2) == 0 ? 1 : -1;
//                bool verticalHorizontal = random.Next(2) == 0 ? true : false;

//                if (ship.Size == 1)
//                {
//                    shipsCoordinates.Add(new Field(startX, startY));
//                    continue;
//                }

//                if (ship.Size == 2 && !verticalHorizontal)
//                {

//                    // horizontal
//                    if (XDirection == -1 && startX > 0 && verticalHorizontal)
//                    {
//                        endX = startX - 1;
//                        endY = startY;

//                        shipsCoordinates.Add(new Field(startX, startY));
//                        shipsCoordinates.Add(new Field(endX, endY));
//                        continue;
//                    }

//                    endX = startX + 1;
//                    endY = startY;

//                    shipsCoordinates.Add(new Field(startX, startY));
//                    shipsCoordinates.Add(new Field(endX, endY));
//                    continue;
//                }

//                if (ship.Size == 2 && verticalHorizontal)
//                {
//                    // vertical
//                    if (YDirection == -1 && startX > 0 && !verticalHorizontal)
//                    {
//                        endX = startX;
//                        endY = startY - 1;

//                        shipsCoordinates.Add(new Field(startX, startY));
//                        shipsCoordinates.Add(new Field(endX, endY));
//                        continue;
//                    }

//                    endX = startX;
//                    endY = startY + 1;

//                    shipsCoordinates.Add(new Field(startX, startY));
//                    shipsCoordinates.Add(new Field(endX, endY));
//                    continue;
//                }
//            }

//            return shipsCoordinates;
//        }
//    }
//}
