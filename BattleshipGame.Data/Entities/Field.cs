namespace BattleshipGame.Data.Entities
{
    public class Field
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int ShipSize { get; set; }

        public bool IsEmpty;

        public bool IsHitted;

        public bool IsValid;
    }
}
