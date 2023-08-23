using BattleshipGame.API.Models.Game;
using BattleshipGame.Data.DbContexts;
using BattleshipGame.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Numerics;

namespace BattleshipGame.API.Services.Repositories
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

        public async Task<List<FieldEntity>> GetInsertedFields(List<int> insertedFields, string playerName)
        {
            List<FieldEntity> fields = new List<FieldEntity>();

            for (int coordinate = 0; coordinate < insertedFields.Count; coordinate++)
            {
                FieldEntity field = await GetPlayerFieldAsync
                    (playerName, insertedFields[coordinate], insertedFields[coordinate + 1]);

                fields.Add(field);
                coordinate++;
            }

            return fields;
        }

        public async Task<List<string>> GetCurrentPlayersByFieldsAsync()
        {
            return await _context.Fields
                .Select(p => p.Player.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetHitFields(string playerName)
        {
            List<FieldEntity> playerHitFields = new List<FieldEntity>();
            List<string> fields = new List<string>();

            playerHitFields = await _context.Fields
                .Where(f => f.Player.Name == playerName && f.IsHitted == true)
                .ToListAsync();

            foreach (var field in playerHitFields)
            {
                fields.Add($"{field.X}, {field.Y}");
            }

            return fields;
        }

        public async Task<bool> AddFieldAsync(Field field, PlayerEntity player)
        {
            _context.Fields.Add(
                new FieldEntity()
                {
                    X = field.X,
                    Y = field.Y,
                    ShipSize = field.ShipSize,
                    IsEmpty = field.IsEmpty,
                    IsHitted = field.IsHitted,
                    IsValid = field.IsValid,
                    Player = player,
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
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
