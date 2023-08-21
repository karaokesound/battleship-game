using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface IGameRepository
    {
        Task AddNewGameAsync(PlayerEntity player1, PlayerEntity player2);

        Task<List<int>> GetPlayersIds();

        Task<bool> SaveChangesAsync();
    }
}
