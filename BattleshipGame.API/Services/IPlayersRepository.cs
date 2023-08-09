using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface IPlayersRepository
    {
        Task<Player> GetPlayer(int playerId);

        Task<IEnumerable<Player>> GetPlayers();

        //Task<Player> UpdatePlayer(int playerId);

        //Task<Player> PartiallyUpdatePlayer(int playerId);

        //Task<Player> DeletePlayer(int playerId);
    }
}
