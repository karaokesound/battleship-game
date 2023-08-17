using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public interface IGeneratingService
    {
        List<Field> GenerateFields(int XFields, int YFields, string player);

        List<Ship> GenerateShips(int numberOfShips);

        string GenerateGameBoard(List<Field> fields, int XFields, int YFields);
    }
}
