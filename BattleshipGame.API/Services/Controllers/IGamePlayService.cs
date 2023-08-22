using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Services.Controllers
{
    public interface IGamePlayService
    {
        Task<List<PlayerEntity>> GetPlayers();

        Task<string> ValidatePlayersAndCoordinates(string playerName, string coordinates);

        Task<List<string>> UpdatePlayerFields(string playerName, string coordinates);

        Task<string> ValidateOpponentTurn();

        Task<List<string>> SetRandomShotByOpponent();
    }
}