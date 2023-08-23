namespace BattleshipGame.API.Models.Game
{
    public class Ship
    {
        public int Size { get; set; }

        public int Id { get; set; }

        public Ship(int size, int id)
        {
            Size = size;
            Id = id;
        }
    }
}
