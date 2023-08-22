using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.API.Services.Repositories
{
    public class PlayerRepository : IPlayersRepository
    {
        private readonly BattleshipGameDbContext _context;

        public PlayerRepository(BattleshipGameDbContext context)
        {
            _context = context;
        }

        public async Task<PlayerEntity?> GetPlayerAsync(int playerId)
        {
            return await _context.Players
                .FirstOrDefaultAsync(i => i.Id == playerId);
        }

        public async Task<PlayerEntity?> GetPlayerByNameAsync(string playerName)
        {
            return await _context.Players
                .FirstOrDefaultAsync(p => p.Name == playerName);
        }

        public async Task<PlayerEntity?> GetRandomPlayerAsync(string player1)
        {
            PlayerEntity? firstPlayer = _context.Players.FirstOrDefault(p => p.Name == player1);

            List<int> playersIds = await _context.Players
                .Select(p => p.Id)
                .ToListAsync();

            if (playersIds == null || playersIds.Count == 0 || firstPlayer == null) return null;

            int minId = playersIds.Min();
            int maxId = playersIds.Max();

            Random random = new Random();
            int randomId;

            do
            {
                randomId = random.Next(minId, maxId);
            }
            while (randomId == firstPlayer.Id);

            return await _context.Players
            .FirstOrDefaultAsync(p => p.Id == randomId);
        }

        public async Task<IEnumerable<PlayerEntity>> GetPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<bool> CreatePlayerAsync(PlayerEntity player)
        {
            var doesPlayerNameExist = await _context.Players
                .AnyAsync(n => n.Name == player.Name);

            if (doesPlayerNameExist || player == null) return false;

            _context.Players.Add(player);
            return true;
        }

        public void DeletePlayer(PlayerEntity player)
        {
            _context.Players.Remove(player);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
