namespace BattleshipGame.API.Models.Game
{
    public class Field
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Player { get; set; }

        public int ShipSize { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsHitted { get; set; }

        public bool IsValid { get; set; }

        public Field(int x, int y, string player)
        {
            X = x;
            Y = y;
            Player = player;
            IsEmpty = true;
            IsHitted = false;
            IsValid = true;
            ShipSize = 0;
        }
    }
}
