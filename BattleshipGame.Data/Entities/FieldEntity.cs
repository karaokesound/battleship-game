namespace BattleshipGame.Data.Entities
{
    public class FieldEntity
    {
        public int Id { get; set; }

        public string Player { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int ShipSize { get; set; }

        public bool IsEmpty;

        public bool IsHitted;

        public bool IsValid;
    }
}
