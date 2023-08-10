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

        public async Task<Player?> GetPlayerAsync(int playerId)
        {
            return await _context.Players
                .FirstOrDefaultAsync(i => i.Id == playerId);
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task CreatePlayerAsync(Player player)
        {
            if (player == null) return;

            _context.Players.Add(player);
        }

        public void DeletePlayer(Player player)
        {
            _context.Players.Remove(player);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
