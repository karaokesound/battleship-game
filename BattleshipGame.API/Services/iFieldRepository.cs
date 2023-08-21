using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface iFieldRepository
    {
        Task<FieldEntity> GetPlayerFieldAsync(string player, int x, int y);

        Task<List<FieldEntity>> GetPlayerFieldsAsync(string player);

        Task<List<string>> GetCurrentPlayersByFieldsAsync();

        Task<bool> AddFieldAsync(FieldEntity field, PlayerEntity player);

        void UpdateFields(List<FieldEntity> hittedFields);

        void DeleteAllFields();

        Task<bool> SaveChangesAsync();
    }
}
