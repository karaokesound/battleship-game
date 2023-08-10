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

        public async Task<bool> CreatePlayerAsync(Player player)
        {
            var doesPlayerNameExist = await _context.Players
                .AnyAsync(n => n.Name == player.Name);

            if (doesPlayerNameExist || player == null) return false; 

            _context.Players.Add(player);
            return true;
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
