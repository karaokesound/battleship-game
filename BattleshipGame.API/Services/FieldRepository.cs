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

        public async Task<Field?> GetFieldAsync(int x, int y)
        {
            return await _context.Fields.FirstOrDefaultAsync(xy => xy.X == x && xy.Y == y);
        }

        public async Task<IEnumerable<Field>> GetFieldsAsync()
        {
            return await _context.Fields.ToListAsync();
        }

        public async Task<bool> AddFieldAsync(int x, int y, int shipSize, bool isEmpty, bool isHitted, bool isValid)
        {
            var field = await _context.Fields.FirstOrDefaultAsync(f => f.X == x && f.Y == y);

            if (field != null) return false;

            _context.Fields.Add(
                new Field()
                {
                    X = x,
                    Y = y,
                    ShipSize = shipSize,
                    IsEmpty = isEmpty,
                    IsHitted = isHitted,
                    IsValid = isValid,
                });

            return true;
        }

        public void DeleteField(Field field)
        {
            _context.Fields.Remove(field);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
