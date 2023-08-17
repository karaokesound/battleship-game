using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleshipGame.API.Services
{
    public class FieldRepository : iFieldRepository
    {
        private readonly BattleshipGameDbContext _context;

        public FieldRepository(BattleshipGameDbContext context)
        {
            _context = context;
        }

        public async Task<List<FieldEntity>> GetPlayerFieldsAsync(string player)
        {
            return await _context.Fields
                .Where(f => f.Player == player)
                .ToListAsync();
        }

        public async Task<List<string>> GetCurrentPlayersByFieldsAsync()
        {
            return await _context.Fields
                .Select(p => p.Player)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AddFieldAsync(int x, int y, int shipSize, bool isEmpty, bool isHitted, bool isValid, string player)
        {
            var field = await _context.Fields.FirstOrDefaultAsync(f => f.X == x && f.Y == y);

            if (field != null) return false;

            _context.Fields.Add(
                new FieldEntity()
                {
                    X = x,
                    Y = y,
                    ShipSize = shipSize,
                    IsEmpty = isEmpty,
                    IsHitted = isHitted,
                    IsValid = isValid,
                    Player = player,
                });

            return true;
        }

        public void DeleteAllFields()
        {
            List<FieldEntity> allFields = _context.Fields.ToList();
            
            foreach (var field in allFields)
            {
                _context.Remove(field);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
