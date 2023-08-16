using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public interface IGeneratingService
    {
        List<Field> GenerateFields(int XFields, int YFields);

        List<Ship> GenerateShips(int numberOfShips);

        void GenerateGameBoard(List<Field> fields, int XFields, int YFields);
    }
}
