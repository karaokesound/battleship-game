using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public class GameRepository : IGameRepository
    {
        private readonly BattleshipGameDbContext _context;

        public GameRepository(BattleshipGameDbContext context)
        {
            _context = context;
        }

        public async Task AddNewGameAsync(PlayerEntity player1, PlayerEntity player2, List<FieldEntity> player1Board,
            List<FieldEntity> player2Board)
        {
            _context.Games.Add(new GameEntity()
            {
                Player1Id = player1.Id,
                Player2Id = player2.Id
            });
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
