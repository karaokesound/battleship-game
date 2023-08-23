using BattleshipGame.API.Models.Game;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.Logic.Services
{
    public interface IValidationService
    {
        bool OneFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username);

        bool TwoFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username);

        bool ThreeFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username);

        bool FourFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields, string username);

        List<int> ValidateCoordsFormatAndReturnId(string coordinates);

        List<string> ValidateIfFieldsWereHit(List<FieldEntity> fields);
    }
}
