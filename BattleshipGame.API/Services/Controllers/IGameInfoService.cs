namespace BattleshipGame.API.Services.Controllers
{
    public interface IGameInfoService
    {
        Task<bool> ValidatePlayers(string playerName);

        Task<string> DisplayGameBoard(string playerName);
    }
}
