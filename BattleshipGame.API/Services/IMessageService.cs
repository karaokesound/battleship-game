using BattleshipGame.Data.Entities;
using static BattleshipGame.API.Services.Controllers.GamePlayService;

namespace BattleshipGame.API.Services
{
    public interface IMessageService
    {
        string PlayerNotFoundMessage();

        string UserCreatingError();

        string Delete();

        string StartGameError();

        string StartGameSuccess(string opponent);

        string GameBoardNotFound();

        string ShootError(string opponent, string key);

        string ShotSuccess(int quantity, List<string> hitShipsCoords);

        string ShotMissed(List<PlayerEntity> players, string playerName);

        string InvalidOperation(List<PlayerEntity> players, string playerName);

        CombinedResponseData AdjustResponseByPlayerName(List<PlayerEntity> players, string playerName,
            List<string> hitShipsCoords);
    }
}
