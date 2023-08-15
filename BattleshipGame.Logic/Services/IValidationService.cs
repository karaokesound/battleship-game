using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public interface IValidationService
    {
        bool OneFieldShipValidation(Field selectedField, List<Field> allFields);

        bool TwoFieldShipValidation(List<Field> selectedFields, List<Field> allFields);
    }
}
