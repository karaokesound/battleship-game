using BattleshipGame.API.Models;

namespace BattleshipGame.API.Stores
{
    public class PlayersDataStore
    {
        public static PlayersDataStore Current { get; } = new PlayersDataStore();

        public List<PlayerDto> Players { get; set; }

        public PlayersDataStore()
        {
            Players = new List<PlayerDto>()
            {
                new PlayerDto() { Id = 1, Name = "karaokesound", City = "Gdynia" },
                new PlayerDto() { Id = 2, Name = "nosia789", City = "Gdynia"},
                new PlayerDto() { Id = 3, Name = "pariparowa", City = "Gdynia Pogorze"},
                new PlayerDto() { Id = 4, Name = "ostrorzne", City = "Bartoszyce"},
            };
        }
    }
}
