namespace BattleshipGame.Data.Entities
{
    public class FieldEntity
    {
        public int Id { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int ShipSize { get; set; }

        public int ShipId { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsHitted { get; set; }

        public bool IsValid { get; set; }

        public PlayerEntity Player { get; set; } = null!;

        public int PlayerId { get; set; }
    }
}
