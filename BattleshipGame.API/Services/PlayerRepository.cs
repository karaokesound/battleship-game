using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.API.Services
{
    public class PlayerRepository : IPlayersRepository
    {
        private readonly BattleshipGameDbContext _context;

        public PlayerRepository(BattleshipGameDbContext context)
        {
            _context = context;
        }

        public async Task<Player?> GetPlayer(int playerId)
        {
            return await _context.Players
                .FirstOrDefaultAsync(i => i.Id == playerId);
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        //public Task<Player> UpdatePlayer(int playerId)
        //{

        //}

        //public Task<Player> PartiallyUpdatePlayer(int playerId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Player> DeletePlayer(int playerId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
