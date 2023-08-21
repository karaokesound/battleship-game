using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface IPlayersRepository
    {
        Task<PlayerEntity> GetPlayerAsync(int playerId);

        Task<PlayerEntity> GetPlayerByNameAsync(string playerName);

        Task<PlayerEntity?> GetRandomPlayerAsync(string player1);

        Task<IEnumerable<PlayerEntity>> GetPlayersAsync();

        Task<bool> CreatePlayerAsync(PlayerEntity player);

        void DeletePlayer(PlayerEntity player);

        Task<bool> SaveChangesAsync();
    }
}
