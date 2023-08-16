using BattleshipGame.API.Models.Game;
using BattleshipGame.Logic.Models.Game;
using System.Text;

namespace BattleshipGame.Logic.Services
{
    public interface IGeneratingService
    {
        List<Field> GenerateFields(int XFields, int YFields);

        List<Ship> GenerateShips(int numberOfShips);

        string GenerateGameBoard(List<Field> fields, int XFields, int YFields);
    }
}
