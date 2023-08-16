using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services
{
    public interface iFieldRepository
    {
        Task<Field?> GetFieldAsync(int x, int y);

        Task<IEnumerable<Field>> GetFieldsAsync();

        Task<bool> AddFieldAsync(int x, int y, int shipSize, bool isEmpty, bool isHitted, bool isValid);

        void DeleteField(Field field);

        Task<bool> SaveChangesAsync();
    }
}
