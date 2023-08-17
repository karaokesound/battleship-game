namespace BattleshipGame.Data.Entities
{
    public class GameEntity
    {
        public int Id { get; set; }

        public PlayerEntity Player1 { get; set; }

        public PlayerEntity Player2 { get; set; }

        public ICollection<FieldEntity> Player1Board { get; set; }

        public ICollection<FieldEntity> Player2Board { get; set; }

        public GameEntity()
        {
            Player1Board = new List<FieldEntity>();
            Player2Board = new List<FieldEntity>();
        }
    }
}
