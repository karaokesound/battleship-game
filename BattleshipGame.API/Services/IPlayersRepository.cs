using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface IPlayersRepository
    {
        Task<Player> GetPlayerAsync(int playerId);

        Task<Player> GetPlayerByNameAsync(string username);

        Task<IEnumerable<Player>> GetPlayersAsync();

        Task<bool> CreatePlayerAsync(Player player);

        void DeletePlayer(Player player);

        Task<bool> SaveChangesAsync();
    }
}
