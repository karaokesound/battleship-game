namespace BattleshipGame.API.Models.Game
{
    public class Field
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Username { get; set; }

        public int ShipSize { get; set; }

        public bool IsEmpty;

        public bool IsHitted;

        public bool IsValid;

        public Field(int x, int y, string username)
        {
            X = x;
            Y = y;
            Username = username;
            IsEmpty = true;
            IsHitted = false;
            IsValid = true;
            ShipSize = 0;
        }
    }
}
