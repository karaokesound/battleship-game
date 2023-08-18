using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface IGameRepository
    {
        Task AddNewGameAsync(PlayerEntity player1, PlayerEntity player2, List<FieldEntity> player1Board,
            List<FieldEntity> player2Board);
    }
}
