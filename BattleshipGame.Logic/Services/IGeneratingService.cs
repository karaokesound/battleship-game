using BattleshipGame.API.Models.Game;

namespace BattleshipGame.Logic.Services
{
    public interface IGeneratingService
    {
        List<Field> GeneratePlayerFields(int XFields, int YFields, string player);

        List<Ship> GenerateShips(int numberOfShips);

        string DisplayGameBoard(List<Field> fields, int XFields, int YFields);

        List<int> GenerateRandomCoordinates(int amount);
    }
}
