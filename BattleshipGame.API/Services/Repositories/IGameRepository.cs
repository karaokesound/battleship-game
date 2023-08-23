using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services.Repositories
{
    public interface IGameRepository
    {
        Task AddNewGameAsync(PlayerEntity player1, PlayerEntity player2);

        Task<List<int>> GetPlayersIds();

        void DeleteAllGames();

        Task<bool> SaveChangesAsync();
    }
}
