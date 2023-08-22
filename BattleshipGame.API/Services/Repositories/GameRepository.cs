using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.API.Services.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly BattleshipGameDbContext _context;

        public GameRepository(BattleshipGameDbContext context)
        {
            _context = context;
        }


        public async Task AddNewGameAsync(PlayerEntity player1, PlayerEntity player2)
        {
            _context.Games.Add(new GameEntity()
            {
                Player1Id = player1.Id,
                Player2Id = player2.Id
            });
        }

        public async Task<List<int>> GetPlayersIds()
        {
            List<int> playersIds = new List<int>
            {
                _context.Games
                .Select(p => p.Player1Id)
                .FirstOrDefault(),

                _context.Games
                .Select(p => p.Player2Id)
                .FirstOrDefault()
            };

            return playersIds;
        }

        public void DeleteAllGames()
        {
            List<GameEntity> games = _context.Games.ToList();

            foreach (var game in games)
            {
                _context.Games.Remove(game);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
