using BattleshipGame.API.Models.Game;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services.Repositories
{
    public interface IFieldRepository
    {
        Task<FieldEntity> GetPlayerFieldAsync(string player, int x, int y);

        Task<List<FieldEntity>> GetPlayerFieldsAsync(string player);

        Task<List<FieldEntity>> GetInsertedFields(List<int> insertedFields, string playerName);

        Task<List<string>> GetCurrentPlayersByFieldsAsync();

        Task<List<string>> GetHitFields(string playerName);

        Task<int> GetNumberOfFieldsWithShipId(FieldEntity field);

        Task<bool> AddFieldAsync(Field field, PlayerEntity player);

        void UpdateFields(List<FieldEntity> hittedFields);

        void UpdateField(FieldEntity hittedField);

        void DeleteAllFields();

        Task<bool> SaveChangesAsync();
    }
}
