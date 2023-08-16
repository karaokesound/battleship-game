using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public interface IValidationService
    {
        bool OneFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields);

        bool TwoFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields);

        bool ThreeFieldShipValidation(int startX, int startY, int endX, int endY, List<Field> allFields);
    }
}
