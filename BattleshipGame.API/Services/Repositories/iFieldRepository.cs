using BattleshipGame.API.Models.Game;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services.Repositories
{
    public interface iFieldRepository
    {
        Task<FieldEntity> GetPlayerFieldAsync(string player, int x, int y);

        Task<List<FieldEntity>> GetPlayerFieldsAsync(string player);

        Task<List<FieldEntity>> GetInsertedFields(List<int> insertedFields, string opponent);

        Task<List<string>> GetCurrentPlayersByFieldsAsync();

        Task<bool> AddFieldAsync(Field field, PlayerEntity player);

        void UpdateFields(List<FieldEntity> hittedFields);

        void DeleteAllFields();

        Task<bool> SaveChangesAsync();
    }
}
