namespace BattleshipGame.API.Models.Game
{
    public class Ship
    {
        public int Size { get; set; }

        public Ship(int size)
        {
            Size = size;
        }
    }
}
