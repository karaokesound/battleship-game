namespace BattleshipGame.Data.Entities
{
    public class Field
    {
        public int Id { get; set; }

        public string Player { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int ShipSize { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsHitted { get; set; }

        public bool IsValid { get; set; }
    }
}
