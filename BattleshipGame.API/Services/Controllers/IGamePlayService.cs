using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services.Controllers
{
    public interface IGamePlayService
    {
        Task<List<PlayerEntity>> GetPlayers();

        Task<string> PlayersAndCoordsNullCheck(string playerName, string coordinates);

        Task<List<string>> UpdatePlayerFields(string playerName, string coordinates);

        Task<string> FlagCheck(int value);

        Task<List<string>> SetRandomShootAndUpdateFields();

        Task<string> RefreshGameBoard();
    }
}