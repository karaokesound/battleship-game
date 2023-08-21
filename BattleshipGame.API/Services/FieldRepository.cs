using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace BattleshipGame.API.Services
{
    public class FieldRepository : iFieldRepository
    {
        private readonly BattleshipGameDbContext _context;

        public FieldRepository(BattleshipGameDbContext context)
        {
            _context = context;
        }

        public async Task<FieldEntity> GetPlayerFieldAsync(string player, int x, int y)
        {
            var field = await _context.Fields
                .FirstOrDefaultAsync(f => f.Player.Name == player && f.X == x && f.Y == y);

            if (field == null) return null;

            return field;
        }

        public async Task<List<FieldEntity>> GetPlayerFieldsAsync(string player)
        {
            return await _context.Fields
                .Where(f => f.Player.Name == player)
                .ToListAsync();
        }

        public async Task<List<string>> GetCurrentPlayersByFieldsAsync()
        {
            return await _context.Fields
                .Select(p => p.Player.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<bool> AddFieldAsync(FieldEntity field, PlayerEntity player)
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
                });

            return true;
        }

        public void UpdateFields(List<FieldEntity> hittedFields)
        {
            var fieldsToUpdate = new List<FieldEntity>();

            foreach (var field in hittedFields)
            {
                var xy = _context.Fields
                .FirstOrDefault(f => f.Player == field.Player && f.X == field.X && f.Y == field.Y);

                xy.IsHitted = true;
            }
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
