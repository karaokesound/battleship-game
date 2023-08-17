namespace BattleshipGame.Data.Entities
{
    public class GameEntity
    {
        public int Id { get; set; }

        public PlayerEntity Player1 { get; set; } = null!;

        public PlayerEntity Player2 { get; set; } = null!;

        public List<FieldEntity> Player1Board { get; set; }

        public List<FieldEntity> Player2Board { get; set; }
    }
}
