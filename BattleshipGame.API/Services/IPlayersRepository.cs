using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface IPlayersRepository
    {
        Task<Player> GetPlayerAsync(int playerId);

        Task<IEnumerable<Player>> GetPlayersAsync();

        Task CreatePlayerAsync(Player player);

        void DeletePlayer(Player player);

        Task<bool> SaveChangesAsync();
    }
}
